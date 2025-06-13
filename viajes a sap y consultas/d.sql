select * from ConfiguracionWS;
select * from ConfiguracionWS where nombre = 'status_credito' and status = 1;

select * from Usuarios where numEmpleado in(306528);

-- tabla de cotizacion de carrito de la app
select * from CotizacionMaster;
-- cotizacion detalleColor
select * from cotizacionDetalle;

select * from Sucursales where idSucursal = 113;

select * from Usuarios where idSucursal = 113;

-- mi usuario es usuario administrador de tipo 1 ver todo
select * from Usuarios where numEmpleado in(306528);

-- usuario gerente regional 
select * from Usuarios where numEmpleado in(44246);

-- usuario gerente 
select * from Usuarios where numEmpleado in(9494);

-- usuario visitador
select * from Usuarios where numEmpleado in(44244);

SELECT MD5('dave4321');


-- consultas para validad los roles de listar las sucursales de supervisor

select * from RolPermisos;
select * from Permisos;

select * from AppLogsUbicaciones where fechaUnitaria = '2025-04-15';

select now();

select * from ConfiguracionWS;

SELECT DISTINCT al.claveSucursal, s.nombre AS nombreSucursal, us.idUsuario ,us.numEmpleado AS numeroEmpleado, us.nombre, us.aPaterno, us.aMaterno 
                    FROM AppLogsUbicaciones al 
                    JOIN Usuarios us ON al.numeroEmpleado = us.idUsuario 
                    JOIN Sucursales s ON al.claveSucursal = s.idSAP 
                    WHERE al.claveSucursal = 'M538' and fechaUnitaria = '2025-04-24' and al.Tipo = 1 and al.idRol != '1' and al.idRol != '10' and al.idRol != '8';
                    
select * from AppLogsUbicaciones where fechaUnitaria = '2025-06-10';

select * from LogsClienteApp where DATE(Fecha) = '2025-06-10';

SELECT DISTINCT id, direccion, latitud, longitud, horaUnitaria from AppLogsUbicaciones WHERE claveSucursal = 'M538' and numeroEmpleado = 248 and fechaUnitaria = '2025-04-22' and tipo = 0 ORDER BY horaUnitaria;




