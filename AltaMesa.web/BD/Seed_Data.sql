USE AltaMesaDB;
GO

-- Roles
INSERT INTO rol (nombre_rol)
VALUES
    ('Admin'),
    ('Mesero'),
    ('Chef');
GO

-- Usuarios (contra_hash = bcrypt)
-- Passwords: admin123, mesero123, chef123
INSERT INTO usuario (id_rol, nombre_usuario, apellido_usuario, correo_usuario, contra_hash)
VALUES
    (1, 'Admin',    'Sistema',   'admin@altamesa.com',   '$2a$11$.CBAMosx0z9xqO9ltKTweOmRQypwZSvoTIV4BytZKRpuI9cUq1piK'),
    (2, 'Mesero',   'Principal', 'mesero@altamesa.com',  '$2a$11$2tKknoH1kdzfZ21vz3uRTuXlmyemJR/y8MGO7ts92.6f1tVONTfnK'),
    (3, 'Chef',     'Cocina',    'chef@altamesa.com',    '$2a$11$NIpIkMHmspfldeD5tk6xb.iMZ8n23nAMjTVY/JqzD6NfyAH9cMwa2');
GO
