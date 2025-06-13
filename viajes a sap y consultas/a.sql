select * from Usuarios where numEmpleado in(306528);
select * from PdfDocuments;

truncate PdfDocuments;

SELECT suc.idSucursal,suc.nombre as nombre, suc.idSAP, sup.idUsuario FROM Sucursales suc INNER JOIN SupervisorSucursales sup ON suc.idSucursal = sup.idSucursal WHERE sup.idUsuario = "3079" AND suc.status = 1 AND IdSAP IS NOT NULL AND sup.status=1 ORDER BY nombre ASC;

select * from SupervisorSucursales;

-- tabla de cotizacion de carrito de la app
select * from CotizacionMaster;
-- cotizacion detalleColor
select * from cotizacionDetalle;

SELECT COUNT(*) AS total_resultados from CotizacionMaster;

-- cotizaciones por sucursal

SELECT u.nombre, u.aPaterno, u.aMaterno , s.nombre from CotizacionMaster cm
inner join Sucursales s ON cm.idSucursal = s.idSucursal
inner join Usuarios u On cm.idDispositivo = u.idUsuario
where cm.Status = 'V';

-- inner con venta y cotizacion
SELECT 
    COUNT(*) AS total_Ventas, 
    SUM(c.totalCotizacion) AS suma_total_Cotizacion,
    SUM(v.montoTotal) AS suma_total_venta
FROM CotizacionMaster c
join Ventas v ON c.idventa = v.idventa
WHERE c.idventa IS NOT NULL AND c.idventa <> '' AND c.Status = 'V';

-- historial de cotizacion 

SELECT cd.codebar, p.`Sanimex.Product.Description` AS description, cd.cantidad, cd.precioUnitario ,cm.Status FROM cotizacionDetalle cd 
INNER JOIN productos p ON cd.codebar = p.`Sanimex.Product.Code`
				INNER JOIN CotizacionMaster cm ON cd.idCotizacion = cm.idCotizacion
				WHERE cd.idCotizacion = '42';
                
select * from Sucursales where idSucursal = 115;




