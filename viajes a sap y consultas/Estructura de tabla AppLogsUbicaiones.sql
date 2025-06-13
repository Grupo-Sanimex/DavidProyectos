CREATE TABLE AppLogsUbicaciones (
    id INT AUTO_INCREMENT PRIMARY KEY,
    direccion VARCHAR(255) NOT NULL,
    latitud DECIMAL(10, 6) NOT NULL,
    longitud DECIMAL(10, 6) NOT NULL,
    claveSucursal VARCHAR(50) NOT NULL,
    fechaUnitaria DATE NOT NULL,
    horaUnitaria TIME NOT NULL,
    numeroEmpleado INT NOT NULL
);

ALTER TABLE AppLogsUbicaciones ADD UNIQUE (
    direccion, latitud, longitud, claveSucursal, fechaUnitaria, horaUnitaria, numeroEmpleado
);

select * from AppLogsUbicaciones;


DELIMITER $$

CREATE PROCEDURE Sp_AppInsertarUbicacion(
    IN p_direccion VARCHAR(255),
    IN p_latitud DECIMAL(10,6),
    IN p_longitud DECIMAL(10,6),
    IN p_claveSucursal VARCHAR(50),
    IN p_fechaUnitaria DATE,
    IN p_horaUnitaria TIME,
    IN p_numeroEmpleado INT
)
BEGIN
    -- Verificar si ya existe un registro con los mismos valores en la misma fecha y hora
    IF NOT EXISTS (
        SELECT 1 FROM AppLogsUbicaciones 
        WHERE direccion = p_direccion 
        AND latitud = p_latitud 
       AND longitud = p_longitud
        AND claveSucursal = p_claveSucursal
        AND fechaUnitaria = p_fechaUnitaria 
        AND numeroEmpleado = p_numeroEmpleado
    ) THEN
        -- Insertar el nuevo registro si no existe duplicado
        INSERT INTO AppLogsUbicaciones (direccion, latitud, longitud, claveSucursal, fechaUnitaria, horaUnitaria, numeroEmpleado)
        VALUES (p_direccion, p_latitud, p_longitud, p_claveSucursal, p_fechaUnitaria, p_horaUnitaria, p_numeroEmpleado);
    END IF;
END $$

DELIMITER ;

-- Procedimiento para listar todos los registros
DELIMITER $$

CREATE PROCEDURE Sp_ListarUbicaciones()
BEGIN
    SELECT * FROM AppLogsUbicaciones;
END $$

DELIMITER ;

-- Procedimiento para listar un solo registro por ID
DELIMITER $$

CREATE PROCEDURE Sp_ListarUbicacionID(IN p_id INT)
BEGIN
    SELECT * FROM AppLogsUbicaciones WHERE id = p_id;
END $$

DELIMITER ;

select * from AppLogsUbicaciones WHERE fechaUnitaria = '2025-03-31';

SELECT direccion, latitud, longitud, claveSucursal, fechaUnitaria, horaUnitaria, numeroEmpleado
FROM AppLogsUbicaciones
WHERE fechaUnitaria = '2025-02-07'
GROUP BY numeroEmpleado, latitud, longitud, fechaUnitaria, horaUnitaria
ORDER BY numeroEmpleado, fechaUnitaria, horaUnitaria;

select * from Sucursales where idSAP = 'M001';

-- Para obtener solo los valores únicos de claveSucursal en la fecha específica, puedes usar DISTINCT. La consulta sería así:

SELECT DISTINCT claveSucursal
FROM AppLogsUbicaciones
WHERE fechaUnitaria = '2025-02-12';

-- Para obtener solo los valores únicos de numeroEmpleado en la claveSucursal específica, puedes usar DISTINCT. La consulta sería así:

SELECT DISTINCT numeroEmpleado
FROM AppLogsUbicaciones
WHERE claveSucursal = 'M538' and fechaUnitaria = '2025-02-12';

-- aplicando el join para mostrar los visitadores por sucursal activos realizando consultas 

SELECT DISTINCT al.claveSucursal ,al.numeroEmpleado, us.nombre, us.aPaterno, us.aMaterno
FROM AppLogsUbicaciones al
JOIN Usuarios us ON al.numeroEmpleado = us.idUsuario
WHERE al.claveSucursal = 'M538' and fechaUnitaria = '2025-02-12';

-- proc

DELIMITER $$

CREATE PROCEDURE Sp_ObtenerVisitadoresActivos(IN p_claveSucursal VARCHAR(10), IN p_fechaUnitaria VARCHAR(10))
BEGIN
    SELECT DISTINCT al.claveSucursal ,al.numeroEmpleado, us.nombre, us.aPaterno, us.aMaterno
FROM AppLogsUbicaciones al
JOIN Usuarios us ON al.numeroEmpleado = us.idUsuario
WHERE al.claveSucursal = p_claveSucursal and fechaUnitaria = p_fechaUnitaria;
END $$

DELIMITER ;


-- extraer las direcciones para motrar en el mapa 

SELECT DISTINCT id, direccion, latitud, longitud, horaUnitaria
FROM AppLogsUbicaciones
WHERE claveSucursal = 'M001' and numeroEmpleado = '3079' and fechaUnitaria = '2025-02-11'
ORDER BY horaUnitaria;

-- Calcular precio por metro consultar 
select * from productos;

select `Sanimex.Product.Code` AS codigoBarra, `Sanimex.FeatureProduct.SquareMeter` AS metroXCaja, `Sanimex.Sales.Prices.RetailPrice` AS Precio from productos where `Sanimex.Product.Code` = 'C20-02-0-01';
select `Sanimex.Product.Code` AS codigoBarra, `Sanimex.FeatureProduct.SquareMeter` AS metroXCaja, `Sanimex.Sales.Prices.RetailPrice` AS Precio from productos where `Sanimex.Product.Code` = 'G16-32-1-23';

select `Sanimex.FeatureProduct.SquareMeter` AS metroXCaja from productos where `Sanimex.Product.Code` = 'G06-33-1-111';


select * from RolPermisos;

select * from Permisos;
-- contar gerentes 
SELECT COUNT(*) AS total_resultados from Usuarios where idRol = 10 and status = 1;

-- contar gerentes regional
SELECT COUNT(*) AS total_resultados from Usuarios where idRol = 8 and status = 1;

-- para ver el rol de visitador
select * from Usuarios where numEmpleado in(306528);


select * from Usuarios where idRol = 10 and status = 1;

select * from Usuarios where numEmpleado in(304755);

-- para super administrador el rol 1 va ver a los de rol 10
-- 15 gerentes regionales pedro miranda ve 15 regionales

select distinct idSucursal, COUNT(*) AS total_resultados from Usuarios where idRol = 10 and status = 1;

-- ver la clave de las sucursales de los gerentes regionales
select distinct s.idSAP, s.nombre,u.idUsuario, u.numEmpleado, u.nombre, u.aPaterno from Usuarios u 
INNER JOIN Sucursales s ON u.idSucursal = s.idSucursal where u.idRol = 8 and s.idSAP != 'VPRO' and u.status = 1;



  -- para el rol de gerente tienen el rol 8 va ver a los de rol 10
-- los 15 gerentes regionales pueden ver posiblemente a uno o mas de uno de los 54 gerentes

select distinct s.idSAP, s.idSucursal, s.nombre, u.nombre, u.aPaterno from SupervisorSucursales ss 
join Sucursales s ON ss.idSucursal = s.idSucursal
JOIN Usuarios u ON s.idSucursal = u.idSucursal
 where ss.idUsuario = 1573 and ss.status = 1;


-- 54 gerentes tienen el rol 8 ve visitador que es rol 4 o 

-- el gerente ve visitador 

select * from AppLogsUbicaciones;

select * from CotizacionMaster where date(fechaAlta) = '2025-06-05' and idDispositivo = 3236;
select * from cotizacionDetalle where idCotizacion = 2210;

SELECT COUNT(*) AS total_Ventas
FROM CotizacionMaster
WHERE idventa IS NOT NULL AND idventa <> '';
-- cantidad de cotizaciones y total de las cotizaciones
SELECT 
    COUNT(*) AS total_Ventas, 
    SUM(totalCotizacion) AS suma_total_ventas
FROM CotizacionMaster
WHERE idventa IS NOT NULL AND idventa <> '';

-- inner con venta y cotizacion
SELECT 
    COUNT(*) AS total_Ventas, 
    SUM(c.totalCotizacion) AS suma_total_Cotizacion,
    SUM(v.montoTotal) AS suma_total_venta
FROM CotizacionMaster c
join Ventas v ON c.idventa = v.idventa
WHERE c.idventa IS NOT NULL AND c.idventa <> '' AND c.Status = 'V';

select * from cotizacionDetalle where idCotizacion = 2206;

select * from Usuarios where numEmpleado in(306528);
-- 30594 -- encargada de sucursal 
select * from Usuarios where nombre = 'SERGIO' and aPaterno = 'MENESES' and aMaterno = 'BARRON';

select * from Usuarios where idUsuario = 1637;

select * from AppLogsUbicaciones where numeroEmpleado = 3121 and fechaUnitaria = '2025-05-14';

select * from Usuarios where nombre = "ARTURO";

select * from Sucursales where idSucursal = 176;

-- orden puebla
-- gerente 
-- id luis = 1640 
-- Alberto Hernandez = 1637
-- GERARDO ESCOBAR 1635
-- id SERGIO MENESES 3121

-- orden guadalajar

-- revidar login de usuarios en la app
select * from AppLogsUbicaciones where numeroEmpleado = 1635;

-- revisar si realizan cotizaciones
select * from CotizacionMaster where idDispositivo = 1637 and fechaAlta = '2025-03-24';

select * from CotizacionMaster where idDispositivo = 3121;

-- tabla para validar usuario

CREATE TABLE validarUsuarioApp (
    idvalidar INT AUTO_INCREMENT PRIMARY KEY,
    numEmpleado INT NOT NULL,
    fechaInicio DATE NOT NULL,
    horaInicio TIME NOT NULL,
    status Bit default 1
);

select * from CotizacionMaster where idCotizacion = 694;

select * from cotizacionDetalle where idCotizacion = 680;

select * from Sucursales where idSucursal = 64;
     
select * from AppLogsUbicaciones WHERE fechaUnitaria = '2025-05-31';

select * from AppLogsUbicaciones WHERE fechaUnitaria = '2025-05-12' and numeroEmpleado = 3122;

select * from AppLogsUbicaciones where numeroEmpleado = 304600;

select * from LogsClienteApp WHERE DATE(Fecha) = '2025-04-15';


select * from AppLogsUbicaciones WHERE fechaUnitaria = '2025-04-29' and claveSucursal = 'M001';

SELECT DISTINCT id, direccion, latitud, longitud, horaUnitaria from AppLogsUbicaciones WHERE claveSucursal = 'M001' and numeroEmpleado = 3103 and fechaUnitaria = '2025-04-22' and tipo = 0 ORDER BY horaUnitaria;


-- n005;
-- n005;

-- verificar ubicaciones
select * from CotizacionMaster where DATE(fechaAlta) = '2025-04-29';

SELECT idCotizacion, idClienteSAP, totalCotizacion, Status, DATE_FORMAT(fechaAlta, '%Y-%m-%d') AS fecha, DATE_FORMAT(fechaAlta, '%H:%i:%s') AS hora, idventa FROM CotizacionMaster WHERE DATE(fechaAlta) = '2025-04-29' AND idDispositivo = 3079;


SELECT DISTINCT cm.idDispositivo , u.nombre, u.aPaterno, u.aMaterno, u.numEmpleado FROM CotizacionMaster cm INNER JOIN Sucursales s ON cm.idSucursal = s.idSucursal 
                INNER JOIN Usuarios u ON cm.idDispositivo = u.idUsuario
                INNER JOIN SupervisorSucursales sp ON cm.idSucursal = sp.idSucursal
                WHERE sp.idUsuario = 3079 AND DATE(cm.fechaAlta) = '2025-05-07';
                                
-- Estructura de tabla sucursales asignadas

-- idGerente
-- examinar diferencias 


	
