create database sistema_inventario;

-- Tabla 1: ubicaciones
-- Contiene información de ubicación jerárquica.

CREATE TABLE ubicaciones (
    id_ubicacion INT AUTO_INCREMENT PRIMARY KEY,
    zona VARCHAR(50),
    region VARCHAR(50),
    centro VARCHAR(50),
    estado VARCHAR(50),
    sucursal VARCHAR(100) UNIQUE,
    status BIT DEFAULT 1
);


-- Tabla 2: departamentos
-- Departamentos dentro de la empresa.

CREATE TABLE departamentos (
    id_departamento INT AUTO_INCREMENT PRIMARY KEY,
    nombre_departamento VARCHAR(100) UNIQUE,
    status BIT DEFAULT 1
);
-- Tabla 3: empleados
-- Información de empleados.

CREATE TABLE empleados (
    id_empleado INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100),
    apellidoP VARCHAR(100),
    apellidoM VARCHAR(100),
    puesto VARCHAR(100),
    id_departamento INT,
    id_ubicacion INT,
    status BIT DEFAULT 1,
    FOREIGN KEY (id_departamento) REFERENCES departamentos(id_departamento),
    FOREIGN KEY (id_ubicacion) REFERENCES ubicaciones(id_ubicacion)
);

-- Tabla de : rollback
-- Tabla de roles
CREATE TABLE roles (
    id_rol INT AUTO_INCREMENT PRIMARY KEY,
    nombre_rol VARCHAR(50) UNIQUE,
    status BIT DEFAULT 1
);
-- Tabla 4: usuarios
-- Credenciales de acceso de empleados.

CREATE TABLE usuarios (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    id_empleado INT,
    usuario_windows VARCHAR(50),
    usuario_ad VARCHAR(50),
    correo VARCHAR(100) UNIQUE,
    pass VARCHAR(300),
    acceso VARCHAR(50),
    id_rol INT,
    status Bit default 1,
    FOREIGN KEY (id_empleado) REFERENCES empleados(id_empleado),
    FOREIGN KEY (id_rol) REFERENCES roles(id_rol)
);

-- Tabla 5: equipos
-- Información técnica de los equipos.

CREATE TABLE equipos (
    id_equipo INT AUTO_INCREMENT PRIMARY KEY,
    numero_serie VARCHAR(50) UNIQUE,
    etiqueta VARCHAR(50),
    marca VARCHAR(50),
    modelo VARCHAR(50),
    ip VARCHAR(15),
    ram VARCHAR(20),
    disco_duro VARCHAR(20),
    procesador VARCHAR(50),
    so VARCHAR(50),
    equipo_estatus VARCHAR(20),
    empresa VARCHAR(50),
    renovar BOOLEAN,
    fecha_ultima_captura DATE,
    fecha_ultimo_mantto DATE,
    elaboro_responsiva VARCHAR(100), -- cambiar aqui que guarde el id del usuario,
    id_ubicacion INT,
    id_departamento INT,
    id_empleado INT,
    status Bit default 1,
    FOREIGN KEY (id_ubicacion) REFERENCES ubicaciones(id_ubicacion),
    FOREIGN KEY (id_departamento) REFERENCES departamentos(id_departamento),
    FOREIGN KEY (id_empleado) REFERENCES empleados(id_empleado)
);

-- Tabla 6: licencias_office
-- Asignación de licencias Office 365 a equipos.


CREATE TABLE licencias_office (
    id_licencia INT AUTO_INCREMENT PRIMARY KEY,
    cuenta VARCHAR(100) UNIQUE,
    id_equipo INT,
    status Bit default 1,
    FOREIGN KEY (id_equipo) REFERENCES equipos(id_equipo)
);

-- Paso 4: Explicación de las Relaciones
-- Ubicaciones: Relaciona zona, region, centro, estado y sucursal en una tabla para evitar redundancia.
-- Departamentos: Separado para permitir asignaciones consistentes.
-- Empleados: Vincula empleados con departamentos y ubicaciones.
-- Usuarios: Almacena credenciales asociadas a empleados (1:1).
-- Equipos: Contiene toda la información técnica y asignaciones (ubicación, departamento, empleado).
-- Licencias Office: Relaciona cuentas de Office con equipos (1:1 o 1:N según tus necesidades).

-- Paso 5: Consideraciones Adicionales
-- Índices: Agrega índices en columnas frecuentemente consultadas (e.g., numero_serie, sucursal, correo).

CREATE INDEX idx_numero_serie ON equipos(numero_serie);
CREATE INDEX idx_correo ON usuarios(correo);


-- Datos Faltantes: Si algún campo puede ser nulo (e.g., fecha_ultimo_mantto), asegúrate de permitir NULL en la definición.
-- Integridad: Las claves foráneas garantizan que no se inserten datos huérfanos.
-- Paso 6: Próximos Pasos
-- Si necesitas ayuda para:

-- Insertar datos desde los Excel a estas tablas (usando scripts SQL o herramientas como Python con pandas).
-- Crear consultas específicas (reportes, búsquedas).
-- Ajustar el diseño según más requisitos.

select now();