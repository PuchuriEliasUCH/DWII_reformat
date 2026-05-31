CREATE DATABASE AltaMesaDB;
GO

Use AltaMesaDB;

CREATE TABLE rol(
	id_rol INT PRIMARY KEY IDENTITY(1,1),
	nombre_rol VARCHAR(50) NOT NULL,
	CONSTRAINT UQ_nombre_rol UNIQUE (nombre_rol)
);

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

CREATE TABLE mesa(
    id_mesa INT IDENTITY(1,1) PRIMARY KEY,

    numero INT NOT NULL,

    capacidad INT NOT NULL,

    estado VARCHAR(20) NOT NULL DEFAULT 'Disponible',

    CONSTRAINT UQ_mesa_numero UNIQUE(numero),

    CONSTRAINT CK_mesa_capacidad
        CHECK(
            capacidad IN (2,4,6,8)
        ),

    CONSTRAINT CK_mesa_estado
        CHECK(
            estado IN
            (
                'Disponible',
                'Ocupada',
                'Inhabilitada'
            )
        )
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

    requiere_preparacion BIT NOT NULL
        DEFAULT 1,

    estado BIT NOT NULL DEFAULT 1,

    created_at DATETIME NOT NULL  DEFAULT GETDATE(),

    updated_at DATETIME,

    updated_by INT,

    CONSTRAINT CK_producto_precio
        CHECK(precio>=0),

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
        CHECK(
            estado IN
            (
                'Abierto',
                'Enviado a cocina',
                'En preparacion',
                'Parcialmente servido',
                'Cerrado',
                'Anulado'
            )
        ),

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

    precio_unitario DECIMAL(10,2)NOT NULL,

    subtotal DECIMAL(10,2)NOT NULL,

    observacion VARCHAR(255),

    estado_detalle VARCHAR(30) NOT NULL DEFAULT 'Ingresado',

    es_adicional BIT NOT NULL DEFAULT 0,

    fecha_registro DATETIME NOT NULL DEFAULT GETDATE(),

    updated_at DATETIME,

    updated_by INT,

    CONSTRAINT CK_detalle_cantidad CHECK(cantidad>0),

    CONSTRAINT CK_detalle_precio CHECK(precio_unitario>=0),

    CONSTRAINT CK_detalle_subtotal CHECK(subtotal>=0),

    CONSTRAINT CK_estado_detalle CHECK( estado_detalle IN
            (
                'Ingresado',
                'En preparacion',
                'Listo para servir',
                'Entregado',
                'Anulado'
            )
        ),

    CONSTRAINT FK_detalle_pedido FOREIGN KEY(id_pedido) REFERENCES pedido(id_pedido),

    CONSTRAINT FK_detalle_producto FOREIGN KEY(id_producto) REFERENCES producto(id_producto)
);
GO


CREATE TABLE auditoria_estado_detalle_pedido(id_auditoria INT IDENTITY(1,1) PRIMARY KEY,

    id_detalle_pedido INT NOT NULL,

    estado_anterior VARCHAR(30),

    estado_nuevo VARCHAR(30) NOT NULL,

    fecha_cambio DATETIME NOT NULL DEFAULT GETDATE(),

    id_usuario INT NOT NULL,

    observacion VARCHAR(255),

    CONSTRAINT FK_auditoria_detalle
        FOREIGN KEY(id_detalle_pedido)
        REFERENCES detalle_pedido(
            id_detalle_pedido
        ),

    CONSTRAINT FK_auditoria_usuario
        FOREIGN KEY(id_usuario)
        REFERENCES usuario(
            id_usuario
        )
);
GO


INSERT INTO categoria_producto
(
    nombre,
    descripcion
)
VALUES
('Pollos','Platos principales de pollo'),
('Combos','Promociones y combos'),
('Parrillas','Especialidades a la parrilla'),
('Bebidas','Bebidas frías'),
('Adicionales','Complementos'),
('Postres','Postres y dulces');
GO

INSERT INTO producto
(
id_categoria,
nombre,
desc_corta,
desc_completa,
precio,
requiere_preparacion
)
SELECT
id_categoria,
'1/4 Pollo',
'Incluye papas y ensalada',
'Porción de pollo a la brasa acompañada de papas fritas y ensalada fresca',
24.90,
1
FROM categoria_producto
WHERE nombre='Pollos';

INSERT INTO producto
(
id_categoria,
nombre,
desc_corta,
desc_completa,
precio,
requiere_preparacion
)
SELECT
id_categoria,
'1/2 Pollo',
'Incluye papas familiares',
'Media porción de pollo con papas y ensalada',
45.90,
1
FROM categoria_producto
WHERE nombre='Pollos';

INSERT INTO producto
(
id_categoria,
nombre,
desc_corta,
desc_completa,
precio,
requiere_preparacion
)
SELECT
id_categoria,
'Pollo Entero',
'Ideal para compartir',
'Pollo entero acompañado de papas familiares y ensalada',
78.90,
1
FROM categoria_producto
WHERE nombre='Pollos';

GO
--


/* ===========================
COMBOS
=========================== */

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Combo Ejecutivo',
'1/4 pollo + bebida',
'Incluye cuarto de pollo, papas y bebida personal',
29.90,1
FROM categoria_producto
WHERE nombre='Combos';

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Combo Familiar',
'Pollo completo',
'Pollo entero con papas familiares y bebida',
95.90,1
FROM categoria_producto
WHERE nombre='Combos';

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Combo Duo',
'Ideal para dos',
'Medio pollo con bebida y complementos',
58.90,1
FROM categoria_producto
WHERE nombre='Combos';

GO

/* ===========================
PARRILLAS
=========================== */

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Parrilla Mixta',
'Carnes seleccionadas',
'Combinación de pollo, carne y acompañamientos',
42.90,1
FROM categoria_producto
WHERE nombre='Parrillas';

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Anticuchos',
'Tradicional peruano',
'Brochetas acompañadas de papas',
22.90,1
FROM categoria_producto
WHERE nombre='Parrillas';

GO

/* ===========================
BEBIDAS
=========================== */

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Inca Kola 500ml',
'Bebida personal',
'Bebida gaseosa personal',
6.90,0
FROM categoria_producto
WHERE nombre='Bebidas';

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Coca Cola 500ml',
'Bebida personal',
'Bebida gaseosa personal',
6.90,0
FROM categoria_producto
WHERE nombre='Bebidas';

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Chicha Morada',
'Bebida natural',
'Chicha morada artesanal',
8.90,0
FROM categoria_producto
WHERE nombre='Bebidas';

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Limonada',
'Natural',
'Limonada fresca',
8.90,0
FROM categoria_producto
WHERE nombre='Bebidas';

GO

/* ===========================
ADICIONALES
=========================== */

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Papas Extra',
'Porción adicional',
'Papas fritas adicionales',
9.90,1
FROM categoria_producto
WHERE nombre='Adicionales';

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Ensalada Extra',
'Complemento',
'Ensalada adicional',
7.90,1
FROM categoria_producto
WHERE nombre='Adicionales';

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Crema Ají',
'Salsa especial',
'Crema artesanal de ají',
3.90,0
FROM categoria_producto
WHERE nombre='Adicionales';

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Crema Rocoto',
'Salsa picante',
'Crema especial de rocoto',
3.90,0
FROM categoria_producto
WHERE nombre='Adicionales';

GO

/* ===========================
POSTRES
=========================== */

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Tres Leches',
'Postre clásico',
'Bizcocho húmedo con tres leches',
12.90,0
FROM categoria_producto
WHERE nombre='Postres';

INSERT INTO producto
(id_categoria,nombre,desc_corta,desc_completa,precio,requiere_preparacion)
SELECT id_categoria,'Brownie',
'Chocolate artesanal',
'Brownie acompañado de salsa',
11.90,0
FROM categoria_producto
WHERE nombre='Postres';

GO

/****************************
VERIFICAR
****************************/

SELECT
c.nombre,
p.nombre,
p.precio
FROM producto p
INNER JOIN categoria_producto c
ON c.id_categoria=p.id_categoria
ORDER BY
c.nombre,
p.nombre;

GO