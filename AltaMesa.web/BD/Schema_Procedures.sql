USE AltaMesaDB;
GO

/*=========================================================
FUNCIONES
=========================================================*/

CREATE OR ALTER FUNCTION fn_calcular_total_pedido
(
    @id_pedido INT
)
RETURNS DECIMAL(10,2)
AS
BEGIN

    DECLARE @total DECIMAL(10,2);

    SELECT
        @total=
        ISNULL(SUM(subtotal),0)
    FROM detalle_pedido
    WHERE id_pedido=@id_pedido
    AND estado_detalle<>'Anulado';

    RETURN @total;

END;
GO


CREATE OR ALTER FUNCTION fn_productos_pendientes
(
    @id_pedido INT
)
RETURNS INT
AS
BEGIN

DECLARE @cantidad INT;

SELECT
@cantidad=
COUNT(*)
FROM detalle_pedido
WHERE id_pedido=@id_pedido
AND estado_detalle IN
(
'Ingresado',
'En preparacion'
);

RETURN ISNULL(@cantidad,0);

END;
GO


/*=========================================================
CRUD USUARIO
=========================================================*/

CREATE OR ALTER PROC sp_crear_usuario
(
@id_rol INT,
@nombre VARCHAR(50),
@apellido VARCHAR(50),
@correo VARCHAR(100),
@password VARCHAR(255)
)
AS
BEGIN

INSERT INTO usuario
(
id_rol,
nombre_usuario,
apellido_usuario,
correo_usuario,
contra_hash
)
VALUES
(
@id_rol,
@nombre,
@apellido,
@correo,
@password
);

END
GO


CREATE OR ALTER PROC sp_listar_usuarios
AS
BEGIN

SELECT
u.*,
r.nombre_rol
FROM usuario u
INNER JOIN rol r
ON u.id_rol=r.id_rol;

END
GO


CREATE OR ALTER PROC sp_actualizar_usuario
(
@id_usuario INT,
@id_rol INT,
@nombre VARCHAR(50),
@apellido VARCHAR(50),
@correo VARCHAR(100),
@estado BIT
)
AS
BEGIN

UPDATE usuario
SET
id_rol=@id_rol,
nombre_usuario=@nombre,
apellido_usuario=@apellido,
correo_usuario=@correo,
estado=@estado,
update_at=GETDATE()
WHERE id_usuario=@id_usuario;

END
GO


CREATE OR ALTER PROC sp_eliminar_usuario
(
@id_usuario INT
)
AS
BEGIN

UPDATE usuario
SET estado=0
WHERE id_usuario=@id_usuario;

END
GO


/*=========================================================
CRUD MESA
=========================================================*/

CREATE OR ALTER PROC sp_crear_mesa
(
@numero INT,
@capacidad INT
)
AS
BEGIN

INSERT INTO mesa
(
numero,
capacidad
)
VALUES
(
@numero,
@capacidad
);

END
GO


CREATE OR ALTER PROC sp_listar_mesas
AS
BEGIN

SELECT * FROM mesa;

END
GO


CREATE OR ALTER PROC sp_actualizar_mesa
(
@id INT,
@capacidad INT,
@estado VARCHAR(20)
)
AS
BEGIN

UPDATE mesa
SET
capacidad=@capacidad,
estado=@estado
WHERE id_mesa=@id;

END
GO


/*=========================================================
CRUD CATEGORIA
=========================================================*/

CREATE OR ALTER PROC sp_crear_categoria
(
@nombre VARCHAR(50),
@descripcion VARCHAR(150)
)
AS
BEGIN

INSERT INTO categoria_producto
(
nombre,
descripcion
)
VALUES
(
@nombre,
@descripcion
);

END
GO


CREATE OR ALTER PROC sp_actualizar_categoria
(
@id_categoria INT,
@nombre VARCHAR(50),
@descripcion VARCHAR(150)
)
AS
BEGIN

UPDATE categoria_producto
SET nombre = @nombre,
    descripcion = @descripcion,
    updated_at = GETDATE()
WHERE id_categoria = @id_categoria;

END
GO

CREATE OR ALTER PROC sp_desactivar_categoria
(
@id_categoria INT
)
AS
BEGIN

UPDATE categoria_producto
SET estado = 0,
    updated_at = GETDATE()
WHERE id_categoria = @id_categoria;

END
GO

CREATE OR ALTER PROC sp_listar_categoria
AS
BEGIN

SELECT * FROM categoria_producto;

END
GO


/*=========================================================
CRUD PRODUCTO
=========================================================*/

CREATE OR ALTER PROC sp_crear_producto
(
@categoria INT,
@nombre VARCHAR(80),
@corta VARCHAR(80),
@larga VARCHAR(255),
@precio DECIMAL(10,2),
@prep BIT
)
AS
BEGIN

INSERT INTO producto
(
id_categoria,
nombre,
desc_corta,
desc_completa,
precio,
requiere_preparacion
)
VALUES
(
@categoria,
@nombre,
@corta,
@larga,
@precio,
@prep
);

END
GO


CREATE OR ALTER PROC sp_listar_producto
AS
BEGIN

SELECT
p.*,
c.nombre categoria
FROM producto p
INNER JOIN categoria_producto c
ON p.id_categoria=c.id_categoria;

END
GO


/**********************************************************
AUTENTICACION
**********************************************************/

CREATE OR ALTER PROC sp_login
(
@correo VARCHAR(100),
@password VARCHAR(255)
)
AS
BEGIN

SELECT TOP 1
u.id_usuario,
u.nombre_usuario,
u.correo_usuario,
r.nombre_rol
FROM usuario u
INNER JOIN rol r
ON r.id_rol=u.id_rol
WHERE
u.correo_usuario=@correo
AND u.contra_hash=@password
AND u.estado=1;

END
GO


/**********************************************************
NEGOCIO
**********************************************************/

CREATE OR ALTER PROC sp_crear_pedido
(
@mesa INT,
@mesero INT,
@obs VARCHAR(255)=NULL
)
AS
BEGIN

IF EXISTS
(
SELECT 1
FROM pedido
WHERE id_mesa=@mesa
AND estado<>'Cerrado'
)
BEGIN
RAISERROR('Mesa ocupada',16,1)
RETURN
END

INSERT INTO pedido
(
id_mesa,
id_mesero,
estado,
observacion_general
)
VALUES
(
@mesa,
@mesero,
'Abierto',
@obs
);

UPDATE mesa
SET estado='Ocupada'
WHERE id_mesa=@mesa;

SELECT SCOPE_IDENTITY();

END
GO


CREATE OR ALTER PROC sp_agregar_detalle_pedido
(
@pedido INT,
@producto INT,
@cantidad INT,
@obs VARCHAR(255)=NULL
)
AS
BEGIN

DECLARE @precio DECIMAL(10,2);

SELECT @precio=precio
FROM producto
WHERE id_producto=@producto;

INSERT detalle_pedido
(
id_pedido,
id_producto,
cantidad,
precio_unitario,
subtotal,
observacion
)
VALUES
(
@pedido,
@producto,
@cantidad,
@precio,
(@cantidad*@precio),
@obs
);

UPDATE pedido
SET total=dbo.fn_calcular_total_pedido(@pedido)
WHERE id_pedido=@pedido;

END
GO


CREATE OR ALTER PROC sp_agregar_adicional_pedido
(
@pedido INT,
@producto INT,
@cantidad INT
)
AS
BEGIN

EXEC sp_agregar_detalle_pedido
@pedido,
@producto,
@cantidad;

UPDATE detalle_pedido
SET es_adicional=1
WHERE id_detalle_pedido=SCOPE_IDENTITY();

END
GO


CREATE OR ALTER PROC sp_cerrar_pedido
(
@pedido INT
)
AS
BEGIN

IF EXISTS(
SELECT 1
FROM detalle_pedido
WHERE id_pedido=@pedido
AND estado_detalle IN
(
'Ingresado',
'En preparacion'
)
)
BEGIN
RAISERROR('Existen productos pendientes',16,1)
RETURN
END

UPDATE pedido
SET
estado='Cerrado',
fecha_cierre=GETDATE()
WHERE id_pedido=@pedido;

UPDATE mesa
SET estado='Disponible'
WHERE id_mesa=
(
SELECT id_mesa
FROM pedido
WHERE id_pedido=@pedido
);

END
GO


/**********************************************************
COCINA
**********************************************************/

CREATE OR ALTER PROC sp_listar_cola_cocina
AS
BEGIN

SELECT *
FROM detalle_pedido
WHERE estado_detalle
IN
(
'Ingresado',
'En preparacion'
);

END
GO


CREATE OR ALTER PROC sp_cambiar_estado_detalle
(
@detalle INT,
@estado VARCHAR(30)
)
AS
BEGIN

UPDATE detalle_pedido
SET estado_detalle=@estado
WHERE id_detalle_pedido=@detalle;

END
GO


CREATE OR ALTER PROC sp_productos_listos
AS
BEGIN

SELECT *
FROM detalle_pedido
WHERE estado_detalle='Listo para servir';

END
GO


/**********************************************************
AUDITORIA
**********************************************************/

CREATE OR ALTER PROC sp_registrar_auditoria_estado
(
@detalle INT,
@anterior VARCHAR(30),
@nuevo VARCHAR(30),
@usuario INT,
@obs VARCHAR(255)
)
AS
BEGIN

INSERT auditoria_estado_detalle_pedido
(
id_detalle_pedido,
estado_anterior,
estado_nuevo,
id_usuario,
observacion
)
VALUES
(
@detalle,
@anterior,
@nuevo,
@usuario,
@obs
);

END
GO


CREATE OR ALTER PROC sp_historial_estado_detalle
(
@detalle INT
)
AS
BEGIN

SELECT *
FROM auditoria_estado_detalle_pedido
WHERE id_detalle_pedido=@detalle;

END
GO


/**********************************************************
VISTAS
**********************************************************/

CREATE OR ALTER VIEW vw_pedidos_activos
AS
SELECT *
FROM pedido
WHERE estado<>'Cerrado';
GO


CREATE OR ALTER VIEW vw_cola_cocina
AS
SELECT
d.*,
m.numero mesa
FROM detalle_pedido d
INNER JOIN pedido p
ON d.id_pedido=p.id_pedido
INNER JOIN mesa m
ON p.id_mesa=m.id_mesa
WHERE d.estado_detalle
IN
(
'Ingresado',
'En preparacion'
);
GO


CREATE OR ALTER VIEW vw_historial_pedidos
AS
SELECT *
FROM pedido
WHERE estado='Cerrado';
GO