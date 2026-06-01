USE AltaMesaDB;
GO

-- Roles
INSERT INTO rol (nombre_rol)
VALUES
    ('admin'),
    ('mesero'),
    ('chef');
GO

-- Usuarios (contra_hash = bcrypt)
-- Passwords: admin123, mesero123, chef123
INSERT INTO usuario (id_rol, nombre_usuario, apellido_usuario, correo_usuario, contra_hash)
VALUES
    (1, 'Admin',    'Sistema',   'admin@altamesa.com',   '$2a$11$.CBAMosx0z9xqO9ltKTweOmRQypwZSvoTIV4BytZKRpuI9cUq1piK'),
    (2, 'Mesero',   'Principal', 'mesero@altamesa.com',  '$2a$11$2tKknoH1kdzfZ21vz3uRTuXlmyemJR/y8MGO7ts92.6f1tVONTfnK'),
    (3, 'Chef',     'Cocina',    'chef@altamesa.com',    '$2a$11$NIpIkMHmspfldeD5tk6xb.iMZ8n23nAMjTVY/JqzD6NfyAH9cMwa2');
GO

INSERT INTO categoria_producto (nombre, descripcion)
VALUES
    ('Pollos',      'Platos principales de pollo'),
    ('Combos',      'Promociones y combos'),
    ('Parrillas',   'Especialidades a la parrilla'),
    ('Bebidas',     'Bebidas frías'),
    ('Adicionales', 'Complementos'),
    ('Postres',     'Postres y dulces');
GO

/* POLLOS */
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, '1/4 Pollo',   'Incluye papas y ensalada',    'Porción de pollo a la brasa acompañada de papas fritas y ensalada fresca', 24.90, 1 FROM categoria_producto WHERE nombre='Pollos';
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, '1/2 Pollo',   'Incluye papas familiares',    'Media porción de pollo con papas y ensalada',                              45.90, 1 FROM categoria_producto WHERE nombre='Pollos';
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Pollo Entero','Ideal para compartir',        'Pollo entero acompañado de papas familiares y ensalada',                   78.90, 1 FROM categoria_producto WHERE nombre='Pollos';
GO

/* COMBOS */
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Combo Ejecutivo','1/4 pollo + bebida',         'Incluye cuarto de pollo, papas y bebida personal', 29.90, 1 FROM categoria_producto WHERE nombre='Combos';
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Combo Familiar', 'Pollo completo',             'Pollo entero con papas familiares y bebida',       95.90, 1 FROM categoria_producto WHERE nombre='Combos';
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Combo Duo',      'Ideal para dos',             'Medio pollo con bebida y complementos',            58.90, 1 FROM categoria_producto WHERE nombre='Combos';
GO

/* PARRILLAS */
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Parrilla Mixta','Carnes seleccionadas',  'Combinación de pollo, carne y acompañamientos', 42.90, 1 FROM categoria_producto WHERE nombre='Parrillas';
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Anticuchos',    'Tradicional peruano',   'Brochetas acompañadas de papas',               22.90, 1 FROM categoria_producto WHERE nombre='Parrillas';
GO

/* BEBIDAS */
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Inca Kola 500ml','Bebida personal', 'Bebida gaseosa personal',       6.90, 0 FROM categoria_producto WHERE nombre='Bebidas';
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Coca Cola 500ml','Bebida personal', 'Bebida gaseosa personal',       6.90, 0 FROM categoria_producto WHERE nombre='Bebidas';
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Chicha Morada',  'Bebida natural',  'Chicha morada artesanal',        8.90, 0 FROM categoria_producto WHERE nombre='Bebidas';
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Limonada',       'Natural',         'Limonada fresca',                8.90, 1 FROM categoria_producto WHERE nombre='Bebidas';
GO

/* ADICIONALES */
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Papas Extra',   'Porción adicional', 'Papas fritas adicionales',   9.90, 1 FROM categoria_producto WHERE nombre='Adicionales';
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Ensalada Extra','Complemento',       'Ensalada adicional',         7.90, 1 FROM categoria_producto WHERE nombre='Adicionales';
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Crema Ají',     'Salsa especial',    'Crema artesanal de ají',     3.90, 0 FROM categoria_producto WHERE nombre='Adicionales';
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Crema Rocoto',  'Salsa picante',     'Crema especial de rocoto',   3.90, 0 FROM categoria_producto WHERE nombre='Adicionales';
GO

/* POSTRES */
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Tres Leches','Postre clásico',      'Bizcocho húmedo con tres leches', 12.90, 0 FROM categoria_producto WHERE nombre='Postres';
INSERT INTO producto (id_categoria, nombre, desc_corta, desc_completa, precio, requiere_preparacion)
SELECT id_categoria, 'Brownie',    'Chocolate artesanal', 'Brownie acompañado de salsa',     11.90, 0 FROM categoria_producto WHERE nombre='Postres';
GO
