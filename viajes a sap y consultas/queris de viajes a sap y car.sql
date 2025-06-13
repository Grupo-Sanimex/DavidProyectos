-- verificar que este timbrada uuid
select * from Facturas where idventa in (353661216);
-- 3.	Verificar que todos los pagos estén timbrados, ejemplo query.

select * from TimbradoComplementoPago where idventa in (353661216);

-- 4.	Verificar si todas las facturas son VS5 (Ventas pagadas al 100%)
--  o VS7 (Ventas pendientes de pago) en la columna idstatus ejemplo de query:
select * from Ventas where idventa in (
353661216
);

-- verificar que le pago este completo
select * from SAPTransactionTender where idSAPTransaction in(7791461);

-- VS7 verificar si tiene pagos
select * from VentaTipoPagos where idVenta in (353661216);
-- si tiene pagos pero no mostro el pago en la tabla de 
select * from TimbradoComplementoPago where idVenta  in(353661216);
-- deberá agregar la partida a mano en la tabla de 



-- tabla para obtener los idSAPTransaction cuando son VS7
select * from SAPTransactionDetail where idVenta in (353661216);

-- Buscar en la tabla de SAPTransaction la venta 
-- conformar tiket 3 = clave de empresa seguido de la clave de sucursal 210
-- verificar el partnerId el cual describe si es de MN menudeo si es M y numero es Mayoreo
select * from SAPTransaction where ventaorigenid = 'M536-353661216' and enviadoCAR = 0;
-- para solucion para viajar ventas cuando son vs7 se viajan con el idSAPTransaction
select * from SAPTransaction where idSAPTransaction in('7828128') and enviadoCAR = 0;
-- y el idSAPTransaction se debe verificar si no existe ninguno se viajan los id diferentes
-- si no comparar cual si esta viajado



-- NO ARROJA CODIGO DE L CENTRO NI CAUSANTE Y Folios de venta ver en error cual es el problema verficar
-- la direccion el causante es el folio sap --
-- debugerar en pos y verificar que no sea un producto importado solo pueden salir de 
-- santa clara 6.
select * from ordendeVenta where idVenta in(343722084);
-- cambiar la idDireccion en la tabla ordendeVenta que esta en la tabla Direcciones
select * from Ventas where idVenta in(210451089);

select * from Clientes where idCliente = 2135;
-- consultar la iddireccion con 
select * from Direcciones where idCliente = 2135;

-- verificar si el pago esta correcto

select * from DetallePagosPinpadBanorte where idVenta = '331562519';

-- buscar un cupon 
select * from Cupones where codigo in('LPII3PT3WMFDD');
-- venta cancelada  y existe en venta tipo pagos 
-- en la cual encuentras la factura a a que se aplico ese cupon
select * from VentaTipoPagos where claveAutorizacion in('TECYRJJGY5WE');
-- tabla de sap transaction
select * from SAPTransaction where ventaOrigenId in('G403-2403107791');
-- buscar la venta a la que se aplico el pago
select * from SAPTransaction where ventaOrigenId in('M606-3606104568');

-- factura pedido buscar en 
select * from requisiciones where idVenta  in(3118113076);

-- Cuando no da codigo de barra ni folios de entrega el problem es sap verfiicar las respuestas de sap

-- cundo no se puede timbrar y el eror es el valor de campo base debe ser mayor a 0 
-- uno de los productos tiene que ser mayor a 0
select * from Sucursales where idSAP= 'M611'; -- ID SUC 194, Id suc 214
select * from SucursalTraslados where sucPadre = 'm950';