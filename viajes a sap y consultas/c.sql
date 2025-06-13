-- Para obtener solo los valores únicos de claveSucursal en la fecha específica, puedes usar DISTINCT. La consulta sería así:

select idSucursal from SupervisorSucursales where idUsuario = 507 and status = 1;

select idSAP from Sucursales where idSucursal = 100;

select s.idSAP from SupervisorSucursales ss 
join Sucursales s ON ss.idSucursal = s.idSucursal where ss.idUsuario = 505 and ss.status = 1;

SELECT DISTINCT s.idSAP
FROM SupervisorSucursales ss
INNER JOIN Sucursales s ON ss.idSucursal = s.idSucursal
INNER JOIN AppLogsUbicaciones au ON s.idSAP = au.claveSucursal
WHERE ss.idUsuario = 505 AND au.fechaUnitaria = '2025-02-21' AND ss.status = 1;


SELECT DISTINCT claveSucursal
FROM AppLogsUbicaciones
WHERE fechaUnitaria = '2025-02-19';


-- ver historial de cotizaciones 
select * from SupervisorSucursales;

-- con esta consulta el gerente va ver a todos sus vistadores y las cotizaciones del dia seleccionado.
SELECT u.nombre, u.aPaterno, u.aMaterno, u.numEmpleado, cm.idDispositivo FROM CotizacionMaster cm 
INNER JOIN Sucursales s ON cm.idSucursal = s.idSucursal
INNER JOIN Usuarios u ON cm.idDispositivo = u.idUsuario
INNER JOIN SupervisorSucursales sp ON cm.idSucursal = sp.idSucursal
WHERE sp.idUsuario = 505 AND DATE(cm.fechaAlta) = '2025-02-21';

-- con esta consulta el gerente va poder ver cada uno de los id de cotizaciones generados
-- en la cual si id venta es vacio se denotara con la leyenda expirada
-- si idventa es diferente de vacio se denotara con completa
-- con esta misma consulta los Visitadores podran ver sus cotizaciones expiradas y completas
SELECT idCotizacion, totalCotizacion, idClienteSAP , Status, DATE(fechaAlta) AS fecha, time(fechaAlta) AS hora FROM CotizacionMaster
WHERE DATE(fechaAlta) = '2025-02-21' AND idDispositivo = 248;


-- Con esta consulta tanto el gerente como el visitador podran ver los detalles de cada cotizacion 
-- que se genero

select cd.codebar, p.`Sanimex.Product.Description` AS description, cd.cantidad, cd.status from cotizacionDetalle cd
inner join productos p ON cd.codebar = p.`Sanimex.Product.Code`
where idCotizacion = 7;		

select * from productos;


select ss.idSucursal from SupervisorSucursales ss where idUsuario = 505;

select * from Sucursales where idSucursal = 113;

-- tabla de cotizacion de carrito de la app
select * from CotizacionMaster;
-- cotizacion detalleColor
select * from cotizacionDetalle;

select * from AppLogsUbicaciones where fechaUnitaria = '2025-02-25' and idRol = 8;

SELECT cm.idDispositivo , u.nombre, u.aPaterno, u.aMaterno, u.numEmpleado FROM CotizacionMaster cm
			INNER JOIN Sucursales s ON cm.idSucursal = s.idSucursal 
			INNER JOIN Usuarios u ON cm.idDispositivo = u.idUsuario 
			INNER JOIN SupervisorSucursales sp ON cm.idSucursal = sp.idSucursal 
			WHERE sp.idUsuario = 507 AND DATE(cm.fechaAlta) = '2025-02-24';
            

SELECT DISTINCT claveSucursal FROM SupervisorSucursales ss INNER JOIN Sucursales s ON ss.idSucursal = s.idSucursal INNER JOIN AppLogsUbicaciones au ON s.idSAP = au.claveSucursal WHERE ss.idUsuario = 505 AND au.fechaUnitaria = '2025-02-25' AND ss.status = 1 AND idRol = '8';

SELECT DISTINCT al.claveSucursal ,al.numeroEmpleado, us.nombre, us.aPaterno, us.aMaterno FROM AppLogsUbicaciones al JOIN Usuarios us ON al.numeroEmpleado = us.idUsuario
 WHERE al.claveSucursal = 'M538' and fechaUnitaria = '2025-02-25' and al.idRol != '1' and al.idRol != '10' and al.idRol != '8';

SELECT DISTINCT claveSucursal FROM AppLogsUbicaciones WHERE fechaUnitaria = '2025-02-25' AND idRol = 10;

SELECT DISTINCT al.claveSucursal ,al.numeroEmpleado, us.nombre, us.aPaterno, us.aMaterno FROM AppLogsUbicaciones al JOIN Usuarios us ON al.numeroEmpleado = us.idUsuario WHERE al.claveSucursal = 'M001' and fechaUnitaria = '2025-02-25' and al.idRol = 10;

select * from AppLogsUbicaciones where claveSucursal = 'M538' and fechaUnitaria = '2025-02-25';

-- para rol 10
SELECT DISTINCT claveSucursal FROM AppLogsUbicaciones WHERE fechaUnitaria = '2025-02-25' AND idRol = 8;

select * from SupervisorSucursales where idUsuario = 507 and status = 1;

SELECT DISTINCT alu.claveSucursal FROM AppLogsUbicaciones alu
INNER JOIN SupervisorSucursales ss ON alu.numeroEmpleado = ss.idUsuario
INNER JOIN Sucursales s ON ss.idSucursal = s.idSucursal
WHERE ss.idUsuario = 507;

SELECT DISTINCT ss.idSucursal from SupervisorSucursales ss 
INNER JOIN AppLogsUbicaciones alu ON alu.numeroEmpleado = ss.idUsuario
WHERE alu.numeroEmpleado = 507 and ss.status = 1;
-- consulta que funciona
SELECT DISTINCT s.idSAP AS claveSucursal, s.nombre from SupervisorSucursales ss 
INNER JOIN Sucursales s ON ss.idSucursal = s.idSucursal
INNER JOIN AppLogsUbicaciones alu ON s.idSAP = alu.claveSucursal
WHERE ss.idUsuario = 507 and alu.fechaUnitaria = '2025-02-27' and ss.status = 1 and alu.idRol = 8;

-- consulta para ubicacion maps 
SELECT DISTINCT al.claveSucursal ,al.numeroEmpleado, us.nombre, us.aPaterno, us.aMaterno FROM AppLogsUbicaciones al JOIN Usuarios us ON al.numeroEmpleado = us.idUsuario WHERE al.claveSucursal = 'M538' and fechaUnitaria = '2025-02-25' and al.idRol = 8;


select * from SupervisorSucursales where idUsuario = 507 and status = 1;

-- para rol 8 Ubicacion sucursal 
SELECT DISTINCT s.idSAP AS claveSucursal from SupervisorSucursales ss
INNER JOIN Sucursales s ON ss.idSucursal = s.idSucursal 
INNER JOIN AppLogsUbicaciones alu ON s.idSAP = alu.claveSucursal 
WHERE ss.idUsuario = '505' and alu.fechaUnitaria = '2025-02-25'
and ss.status = 1 and  alu.idRol != '1' and alu.idRol != '10' and alu.idRol != '8';

-- para ver los detalles de la cotizacion 
-- para rol 1, 10, 8

-- generar join para los roles 10 y 8

-- tabla de cotizacion de carrito de la app
select * from CotizacionMaster;
-- cotizacion detalleColor
select * from cotizacionDetalle;


SELECT DISTINCT s.idSAP AS claveSucursal from SupervisorSucursales ss
INNER JOIN Sucursales s ON ss.idSucursal = s.idSucursal 
INNER JOIN CotizacionMaster cm ON s.idSucursal = cm.idSucursal 
WHERE ss.idUsuario = '505' and DATE(cm.fechaAlta) = '2025-02-27'
and ss.status = 1;

-- esta consulta es para listar a los vistadores de cada gerente

SELECT DISTINCT cm.idDispositivo , u.nombre, u.aPaterno, u.aMaterno, u.numEmpleado FROM CotizacionMaster cm
INNER JOIN Sucursales s ON cm.idSucursal = s.idSucursal 
INNER JOIN Usuarios u ON cm.idDispositivo = u.idUsuario 
INNER JOIN SupervisorSucursales sp ON cm.idSucursal = sp.idSucursal
WHERE sp.idUsuario = 505 AND DATE(cm.fechaAlta) = '2025-02-26';


-- conuslta para obtener las cotizaciones del vistador 
SELECT idCotizacion, idClienteSAP, totalCotizacion, Status, DATE_FORMAT(fechaAlta, '%Y-%m-%d') AS fecha,
DATE_FORMAT(fechaAlta, '%H:%i:%s') AS hora, idventa FROM CotizacionMaster
WHERE DATE(fechaAlta) = '2025-02-26' AND idDispositivo = 248;

select cd.codebar, p.`Sanimex.Product.Description` AS description, cd.cantidad, cd.status from cotizacionDetalle cd inner join productos p ON cd.codebar = p.`Sanimex.Product.Code` where idCotizacion = 9;

-- para el rol de visitador
SELECT cd.codebar, p.`Sanimex.Product.Description` AS description, cd.cantidad, cd.precioUnitario ,cm.Status FROM cotizacionDetalle cd
INNER JOIN productos p ON cd.codebar = p.`Sanimex.Product.Code`
INNER JOIN CotizacionMaster cm ON cd.idCotizacion = cm.idCotizacion
WHERE cd.idCotizacion = 14;

-- conulta para obtenr datos del cliente 
SELECT cd.codebar, cm.idClienteSAP AS claveCliente, s.idSAP AS centrosCorredor FROM CotizacionMaster cm 
INNER JOIN Sucursales s ON cm.idSucursal = s.idSucursal
INNER JOIN cotizacionDetalle cd ON cm.idCotizacion = cd.idCotizacion
where cm.idCotizacion = '14';

-- consulta para modificar 
SELECT DISTINCT al.claveSucursal, s.nombre ,us.idUsuario , us.numEmpleado, us.nombre, us.aPaterno, us.aMaterno 
FROM AppLogsUbicaciones al 
JOIN Usuarios us ON al.numeroEmpleado = us.idUsuario
JOIN Sucursales s ON al.claveSucursal = s.idSAP
WHERE al.claveSucursal = 'M538' and fechaUnitaria = '2025-02-26' and al.idRol != '1' and al.idRol != '10' and al.idRol != '8';

select * from Usuarios;

-- tabla de cotizacion de carrito de la app
select * from CotizacionMaster where idCotizacion = 24;
-- cotizacion detalleColor
select * from cotizacionDetalle;


SELECT cm.idDispositivo, s.idSAP as claveSucursal, s.nombre AS Sucursal, u.nombre, u.aPaterno, cm.Status, cm.totalCotizacion, cm.idClienteSAP, cm.idventa 
FROM CotizacionMaster cm 
INNER JOIN Usuarios u ON cm.idDispositivo = u.idUsuario 
	INNER JOIN Sucursales s ON cm.idSucursal = s.idSucursal 
                WHERE cm.idSucursal IN (SELECT ss.idSucursal FROM SupervisorSucursales ss 
                WHERE ss.idUsuario = @idGerente and ss.Status = 1) and DATE(cm.fechaAlta) = @Fecha;
                
                
select * from LogsClienteApp where DATE(Fecha) = '2025-06-11';

select * from AppLogsUbicaciones WHERE fechaUnitaria = '2025-06-11';

-- consulta antiduplicados 

ALTER TABLE AppLogsUbicaciones ADD COLUMN hash_dedup VARCHAR(32);
CREATE UNIQUE INDEX uk_location_dedup ON AppLogsUbicaciones (hash_dedup);






