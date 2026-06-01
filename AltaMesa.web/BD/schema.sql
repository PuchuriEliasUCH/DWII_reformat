CREATE DATABASE AltaMesaDB;
GO

USE AltaMesaDB;
GO

/*=========================================================
TABLAS
=========================================================*/

CREATE TABLE rol(
	id_rol INT PRIMARY KEY IDENTITY(1,1),
	nombre_rol VARCHAR(50) NOT NULL,
	CONSTRAINT UQ_nombre_rol UNIQUE (nombre_rol)
);
GO

CREATE TABLE usuario(
	id_usuario INT PRIMARY KEY IDENTITY(1,1),
	id_rol INT NOT NULL,
	nombre_usuario VARCHAR(50) NOT NULL,
	apellido_usuario VARCHAR(50) NOT NULL,
	correo_usuario VARCHAR(100) NOT NULL,
	contra_hash VARCHAR(255) NOT NULL,
	estado BIT NOT NULL DEFAULT 1,
	create_at DATETIME NOT NULL DEFAULT GETDATE(),
	update_at DATETIME,
	update_by INT,
	CONSTRAINT FK_usuario_rol FOREIGN KEY (id_rol) REFERENCES rol(id_rol)
);
GO

CREATE TABLE mesa(
    id_mesa INT IDENTITY(1,1) PRIMARY KEY,
    numero INT NOT NULL,
    capacidad INT NOT NULL,
    estado VARCHAR(20) NOT NULL DEFAULT 'Disponible',
    CONSTRAINT UQ_mesa_numero UNIQUE(numero),
    CONSTRAINT CK_mesa_capacidad
        CHECK(capacidad IN (2,4,6,8)),
    CONSTRAINT CK_mesa_estado
        CHECK(estado IN ('Disponible','Ocupada','Inhabilitada'))
);
GO

CREATE TABLE categoria_producto(
    id_categoria INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    descripcion VARCHAR(150),
    estado BIT NOT NULL DEFAULT 1,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME,
    CONSTRAINT UQ_categoria_nombre UNIQUE(nombre)
);
GO

CREATE TABLE producto(
    id_producto INT IDENTITY(1,1) PRIMARY KEY,
    id_categoria INT NOT NULL,
    nombre VARCHAR(80) NOT NULL,
    desc_corta VARCHAR(80) NOT NULL,
    desc_completa VARCHAR(255) NOT NULL,
    precio DECIMAL(10,2) NOT NULL,
    requiere_preparacion BIT NOT NULL DEFAULT 1,
    estado BIT NOT NULL DEFAULT 1,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME,
    updated_by INT,
    CONSTRAINT CK_producto_precio CHECK(precio>=0),
    CONSTRAINT FK_producto_categoria FOREIGN KEY(id_categoria) REFERENCES categoria_producto(id_categoria)
);
GO

CREATE TABLE pedido(
    id_pedido INT IDENTITY(1,1) PRIMARY KEY,
    id_mesa INT NOT NULL,
    id_mesero INT NOT NULL,
    fecha_pedido DATETIME NOT NULL DEFAULT GETDATE(),
    fecha_cierre DATETIME,
    estado VARCHAR(30) NOT NULL,
    observacion_general VARCHAR(255),
    subtotal DECIMAL(10,2) NOT NULL DEFAULT 0,
    descuento DECIMAL(10,2) NOT NULL DEFAULT 0,
    total DECIMAL(10,2) NOT NULL DEFAULT 0,
    updated_at DATETIME,
    updated_by INT,
    CONSTRAINT CK_pedido_estado
        CHECK(estado IN ('Abierto','Enviado a cocina','En preparacion','Parcialmente servido','Cerrado','Anulado')),
    CONSTRAINT CK_subtotal CHECK(subtotal>=0),
    CONSTRAINT CK_descuento CHECK(descuento>=0),
    CONSTRAINT CK_total CHECK(total>=0),
    CONSTRAINT FK_pedido_mesa FOREIGN KEY(id_mesa) REFERENCES mesa(id_mesa),
    CONSTRAINT FK_pedido_mesero FOREIGN KEY(id_mesero) REFERENCES usuario(id_usuario)
);
GO

CREATE TABLE detalle_pedido(
    id_detalle_pedido INT IDENTITY(1,1) PRIMARY KEY,
    id_pedido INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,
    observacion VARCHAR(255),
    estado_detalle VARCHAR(30) NOT NULL DEFAULT 'Ingresado',
    es_adicional BIT NOT NULL DEFAULT 0,
    fecha_registro DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME,
    updated_by INT,
    CONSTRAINT CK_detalle_cantidad CHECK(cantidad>0),
    CONSTRAINT CK_detalle_precio CHECK(precio_unitario>=0),
    CONSTRAINT CK_detalle_subtotal CHECK(subtotal>=0),
    CONSTRAINT CK_estado_detalle
        CHECK(estado_detalle IN ('Ingresado','En preparacion','Listo para servir','Entregado','Anulado')),
    CONSTRAINT FK_detalle_pedido FOREIGN KEY(id_pedido) REFERENCES pedido(id_pedido),
    CONSTRAINT FK_detalle_producto FOREIGN KEY(id_producto) REFERENCES producto(id_producto)
);
GO

CREATE TABLE auditoria_estado_detalle_pedido(
    id_auditoria INT IDENTITY(1,1) PRIMARY KEY,
    id_detalle_pedido INT NOT NULL,
    estado_anterior VARCHAR(30),
    estado_nuevo VARCHAR(30) NOT NULL,
    fecha_cambio DATETIME NOT NULL DEFAULT GETDATE(),
    id_usuario INT NOT NULL,
    observacion VARCHAR(255),
    CONSTRAINT FK_auditoria_detalle
        FOREIGN KEY(id_detalle_pedido) REFERENCES detalle_pedido(id_detalle_pedido),
    CONSTRAINT FK_auditoria_usuario
        FOREIGN KEY(id_usuario) REFERENCES usuario(id_usuario)
);
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
    SELECT @total = ISNULL(SUM(subtotal),0)
    FROM detalle_pedido
    WHERE id_pedido = @id_pedido
      AND estado_detalle <> 'Anulado';
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
    SELECT @cantidad = COUNT(*)
    FROM detalle_pedido
    WHERE id_pedido = @id_pedido
      AND estado_detalle IN ('Ingresado','En preparacion');
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
    INSERT INTO usuario (id_rol, nombre_usuario, apellido_usuario, correo_usuario, contra_hash)
    VALUES (@id_rol, @nombre, @apellido, @correo, @password);
END
GO

CREATE OR ALTER PROC sp_listar_usuarios
AS
BEGIN
    SELECT u.id_usuario, u.id_rol, u.nombre_usuario, u.apellido_usuario,
           u.correo_usuario, u.contra_hash, u.estado, u.create_at,
           u.update_at, u.update_by, r.nombre_rol
    FROM usuario u
    INNER JOIN rol r ON u.id_rol = r.id_rol;
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
    SET id_rol = @id_rol,
        nombre_usuario = @nombre,
        apellido_usuario = @apellido,
        correo_usuario = @correo,
        estado = @estado,
        update_at = GETDATE()
    WHERE id_usuario = @id_usuario;
END
GO

CREATE OR ALTER PROC sp_eliminar_usuario
(
@id_usuario INT
)
AS
BEGIN
    UPDATE usuario SET estado = 0 WHERE id_usuario = @id_usuario;
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
    INSERT INTO mesa (numero, capacidad) VALUES (@numero, @capacidad);
END
GO

CREATE OR ALTER PROC sp_listar_mesas
AS
BEGIN
    SELECT id_mesa, numero, capacidad, estado FROM mesa;
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
    UPDATE mesa SET capacidad = @capacidad, estado = @estado WHERE id_mesa = @id;
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
    INSERT INTO categoria_producto (nombre, descripcion) VALUES (@nombre, @descripcion);
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
    SET nombre = @nombre, descripcion = @descripcion, updated_at = GETDATE()
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
    SET estado = 0, updated_at = GETDATE()
    WHERE id_categoria = @id_categoria;
END
GO

CREATE OR ALTER PROC sp_listar_categoria
AS
BEGIN
    SELECT id_categoria, nombre, descripcion, estado, created_at, updated_at
    FROM categoria_producto;
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
    INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
    VALUES (@categoria, @nombre, @corta, @larga, @precio, @prep);
END
GO

CREATE OR ALTER PROC sp_listar_producto
AS
BEGIN
    SELECT p.id_producto, p.id_categoria, p.nombre, p.desc_corta, p.desc_completa,
           p.precio, p.requiere_preparacion, p.estado, p.created_at,
           p.updated_at, p.updated_by, c.nombre categoria
    FROM producto p
    INNER JOIN categoria_producto c ON p.id_categoria = c.id_categoria;
END
GO

/*=========================================================
AUTENTICACION
=========================================================*/

CREATE OR ALTER PROC sp_login
(
@correo VARCHAR(100)
)
AS
BEGIN
    SELECT u.id_usuario, u.nombre_usuario, u.correo_usuario, u.contra_hash, r.nombre_rol
    FROM usuario u
    INNER JOIN rol r ON r.id_rol = u.id_rol
    WHERE u.correo_usuario = @correo AND u.estado = 1;
END
GO

/*=========================================================
NEGOCIO
=========================================================*/

CREATE OR ALTER PROC sp_crear_pedido
(
@mesa INT,
@mesero INT,
@obs VARCHAR(255) = NULL
)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM pedido WHERE id_mesa = @mesa AND estado <> 'Cerrado')
    BEGIN
        RAISERROR('Mesa ocupada', 16, 1);
        RETURN;
    END

    INSERT INTO pedido (id_mesa, id_mesero, estado, observacion_general)
    VALUES (@mesa, @mesero, 'Abierto', @obs);

    UPDATE mesa SET estado = 'Ocupada' WHERE id_mesa = @mesa;

    SELECT SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROC sp_agregar_detalle_pedido
(
@pedido INT,
@producto INT,
@cantidad INT,
@obs VARCHAR(255) = NULL
)
AS
BEGIN
    DECLARE @precio DECIMAL(10,2);
    DECLARE @requiere_prep BIT;

    SELECT @precio = precio, @requiere_prep = requiere_preparacion
    FROM producto
    WHERE id_producto = @producto;

    INSERT INTO detalle_pedido (id_pedido, id_producto, cantidad, precio_unitario, subtotal, observacion, estado_detalle)
    VALUES (@pedido, @producto, @cantidad, @precio, (@cantidad * @precio), @obs,
        CASE WHEN @requiere_prep = 1 THEN 'Ingresado' ELSE 'Entregado' END);

    UPDATE pedido SET total = dbo.fn_calcular_total_pedido(@pedido) WHERE id_pedido = @pedido;
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
    EXEC sp_agregar_detalle_pedido @pedido, @producto, @cantidad;
    UPDATE detalle_pedido SET es_adicional = 1 WHERE id_detalle_pedido = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROC sp_cerrar_pedido
(
@pedido INT
)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM detalle_pedido WHERE id_pedido = @pedido AND estado_detalle IN ('Ingresado','En preparacion'))
    BEGIN
        RAISERROR('Existen productos pendientes', 16, 1);
        RETURN;
    END

    UPDATE pedido SET estado = 'Cerrado', fecha_cierre = GETDATE() WHERE id_pedido = @pedido;

    UPDATE mesa SET estado = 'Disponible'
    WHERE id_mesa = (SELECT id_mesa FROM pedido WHERE id_pedido = @pedido);
END
GO

/*=========================================================
COCINA
=========================================================*/

CREATE OR ALTER PROC sp_listar_cola_cocina
AS
BEGIN
    SELECT id_detalle_pedido, id_pedido, id_producto, cantidad,
           precio_unitario, subtotal, observacion, estado_detalle,
           es_adicional, fecha_registro, updated_at, updated_by
    FROM detalle_pedido
    WHERE estado_detalle IN ('Ingresado','En preparacion');
END
GO

CREATE OR ALTER PROC sp_cambiar_estado_detalle
(
@detalle INT,
@estado VARCHAR(30)
)
AS
BEGIN
    UPDATE detalle_pedido SET estado_detalle = @estado WHERE id_detalle_pedido = @detalle;
END
GO

CREATE OR ALTER PROC sp_productos_listos
AS
BEGIN
    SELECT id_detalle_pedido, id_pedido, id_producto, cantidad,
           precio_unitario, subtotal, observacion, estado_detalle,
           es_adicional, fecha_registro, updated_at, updated_by
    FROM detalle_pedido
    WHERE estado_detalle = 'Listo para servir';
END
GO

/*=========================================================
AUDITORIA
=========================================================*/

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
    INSERT INTO auditoria_estado_detalle_pedido (id_detalle_pedido, estado_anterior, estado_nuevo, id_usuario, observacion)
    VALUES (@detalle, @anterior, @nuevo, @usuario, @obs);
END
GO

CREATE OR ALTER PROC sp_historial_estado_detalle
(
@detalle INT
)
AS
BEGIN
    SELECT id_auditoria, id_detalle_pedido, estado_anterior, estado_nuevo,
           fecha_cambio, id_usuario, observacion
    FROM auditoria_estado_detalle_pedido
    WHERE id_detalle_pedido = @detalle;
END
GO

/*=========================================================
VISTAS
=========================================================*/

CREATE OR ALTER VIEW vw_pedidos_activos
AS
    SELECT id_pedido, id_mesa, id_mesero, fecha_pedido, fecha_cierre,
           estado, observacion_general, subtotal, descuento, total,
           updated_at, updated_by
    FROM pedido
    WHERE estado <> 'Cerrado';
GO

CREATE OR ALTER VIEW vw_cola_cocina
AS
    SELECT d.id_detalle_pedido, d.id_pedido, d.id_producto, d.cantidad,
           d.precio_unitario, d.subtotal, d.observacion, d.estado_detalle,
           d.es_adicional, d.fecha_registro, d.updated_at, d.updated_by,
           m.numero mesa
    FROM detalle_pedido d
    INNER JOIN pedido p ON d.id_pedido = p.id_pedido
    INNER JOIN mesa m ON p.id_mesa = m.id_mesa
    WHERE d.estado_detalle IN ('Ingresado','En preparacion');
GO

CREATE OR ALTER VIEW vw_historial_pedidos
AS
    SELECT id_pedido, id_mesa, id_mesero, fecha_pedido, fecha_cierre,
           estado, observacion_general, subtotal, descuento, total,
           updated_at, updated_by
    FROM pedido
    WHERE estado = 'Cerrado';
GO
