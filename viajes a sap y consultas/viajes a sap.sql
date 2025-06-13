-- verificar que este timbrada uuid
select * from Facturas where idventa in (2101137140);
-- En caso de que la factura se quiera timbrar sin uso de cfdi se modifica usoCFDI = S01 y response = error
-- 3.	Verificar que todos los pagos estén timbrados, ejemplo query.

-- En esta tabla encuentras los detalles de la venta con sus produtos -- opcional
select * from VentaDetalles where idVenta in(362286776);


select * from TimbradoComplementoPago where idventa in (350564330);
-- 3205.8200
-- 4.	Verificar si todas las facturas son VS5 (Ventas pagadas al 100%)
--  o VS7 (Ventas pendientes de pago) en la columna idstatus ejemplo de query:

select * from Ventas where idventa in (
2403115898
);

-- VS7 verificar si tiene pagos
select * from VentaTipoPagos where idVenta in (312350608);

-- si tiene pagos pero no mostro el pago en la tabla de 
select * from TimbradoComplementoPago where idVenta  in(2601208896);
-- deberá agregar la partida a mano en la tabla de 

-- tabla para obtener los idSAPTransaction cuando son VS7
select * from SAPTransactionDetail where idVenta in (312350608);

-- verificar que le pago este completo
select * from SAPTransactionTender where idSAPTransaction in(8267379);


-- Buscar en la tabla de SAPTransaction la venta 
-- conformar tiket 3 = clave de empresa seguido de la clave de sucursal 210
-- verificar el partnerId el cual describe si es de MN menudeo si es M y numero es Mayoreo
	select * from SAPTransaction where ventaorigenid = 'M415-341562197'	
	 and enviadoCAR = 0;
 
-- para solucion para viajar ventas cuando son vs7 se viajan con el idSAPTransaction
select * from SAPTransaction where idSAPTransaction in('8016032') and enviadoCAR = 0;
-- y el idSAPTransaction se debe verificar si no existe ninguno se viajan los id diferentes
-- si no comparar cual si esta viajado

-- cuando un cupon esta en 0 se busca que exista el cupon despues en ventaTipoPago se busca por clave autorizacion
select * from VentaTipoPagos where claveAutorizacion in('');

-- NO ARROJA CODIGO DE L CENTRO NI CAUSANTE Y Folios de venta ver en error cual es el problema verficar
-- la direccion el causante es el folio sap --
-- debugerar en pos y verificar que no sea un producto importado solo pueden salir de 
-- santa clara 6.
-- Cuando se quiere volver a generar los folios SAP poner los folios en null y estatus en 1 
-- la direcion que esta en Ventas 
-- en campo idDireccionEnvio debe ser el mismo de ordendeVenta en el campo idDireccion
select * from ordendeVenta where idVenta in(2503124463);

select * from Sucursales where idSAP = 'M410';

-- Ver numero de venta con folio sap
select * from ordendeVenta where folioSap in(0087966396);
--  facturas pedido 
select * from requisiciones where idVenta in(36142630);
-- cambiar la idDireccionEnvio en la tabla ordendeVenta que esta en la tabla Direcciones
select * from Ventas where idVenta in(350562058);

select * from Clientes where idCliente = 981834;
-- consultar la iddireccion con 
select * from Direcciones where idCliente = 981834;
select * from Direcciones where rfc in('MAGA840118GN1');

-- verificar si el pago esta correcto

select * from DetallePagosPinpadBanorte where idVenta = '331562519';

-- buscar un cupon 
select * from Cupones where codigo in('LXZC5GHPSH');
-- de esta tabla se obtienen los codigos de cupon ingresando la venta
select * from Cupones where idVenta in(312350608);
-- venta cancelada  y existe en venta tipo pagos
-- en la cual encuentras la factura a a que se aplico ese cupon
select * from VentaTipoPagos where claveAutorizacion in('TDMBRSM5RNB');
-- tabla de sap transaction
select * from SAPTransaction where ventaOrigenId in('G702-270231970');
-- buscar la venta a la que se aplico el pago
select * from SAPTransaction where ventaOrigenId in('M001-300142280');

-- factura pedido buscar en 
select * from requisiciones where idVenta  in(3118113076);

-- Cuando no da codigo de barra ni folios de entrega el problem es sap verfiicar las respuestas de sap

-- cundo no se puede timbrar y el eror es el valor de campo base debe ser mayor a 0 
-- uno de los productos tiene que ser mayor a 0
-- M425
select * from Sucursales where idSAP= 'M425'; -- ID SUC 194, Id suc 214, id 213, id 66, id 194, id 219, 148,107, 128
select * from SucursalTraslados where sucPadre = 'm950';

-- tabla de cotizacion de carrito de la app
select * from CotizacionMaster;
-- cotizacion detalleColor
select * from cotizacionDetalle;

delete from cotizacionDetalle where idCotizacion = 14; 

-- usuario 

select * from Usuarios where idUsuario = 3079;
select numEmpleado from Usuarios where idUsuario = 3079;

-- mapas

select * from AgrupadorClave;

-- modicacion de credenciales
select * from Usuarios where numEmpleado in(30306979);

select * from Usuarios where numEmpleado in(306528);

-- 308998
-- 306528
-- 401960
-- 305494 iraiz hernandez tmk
select contrasena from Usuarios where numEmpleado in(306528);

UPDATE Usuarios  SET contrasena = MD5('Mj') where numEmpleado = 308998;

SELECT MD5('123456');
select * from Usuarios where nombre = 'ALMA' and aPaterno = 'SUSANA';

 select * from Usuarios;
    
    select * from Sucursales where nombre like '%chalco%';
    
     select * from Sucursales where idSucursal = 157;
    
    select * from Clientes where idCliente = '8707';
    -- cajas
        select * from cajas where iphost = '192.168.88.1'; -- anterior clave sap de 192.168.178.41 -- clave MD
        select * from cajas where idSucursal = 213;

    -- eliminacion de usuario
    select * from cajas;
select * from Usuarios where nombre = "GUADALUPE" and aPaterno = "ROMERO";

select * from RolPermisos;
select * from Permisos;

-- cuando ecommerce no pueda acceder se agrega a la tabla de supervisores.

select * from SupervisorSucursales where idUsuario = 3079;

-- C20-08-0-11

SELECT sucHijoSap FROM SucursalTraslados WHERE sucPadre = @centro;


    

