using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Producto
{
    public class PrecioProducto
    {
        public int id { get; set; }
        public string? SanimexProductCode { get; set; }
        public string? SanimexProductDescription { get; set; }
        public string? SanimexClassificationProductProvider { get; set; }
        public string? SanimexClassificationProductProviderCode { get; set; }
        public string? SanimexClassificationProductClassification { get; set; }
        public int SanimexFeatureProductWeight { get; set; }
        public string? SanimexFeatureProductColor { get; set; }
        public string? SanimexFeatureProductFormat { get; set; }
        public int? SanimexSalesAvailability { get; set; }
        public string? SanimexSalesComplementComplementCode0 { get; set; }
        public string? SanimexSalesPricesRetailPrice { get; set; }
        public double? SanimexSalesPricesWholesalePrice { get; set; }
        public string? SanimexSalesPricesTax { get; set; }
        public double? SanimexSalesPricesPriceByShop0Shop { get; set; }
        public double? SanimexSalesPricesPriceGSARetailPrice { get; set; }
        public double? SanimexSalesPricesPriceGSATax { get; set; }
        public double? SanimexSalesPricesPriceGSAWholesalePrice { get; set; }
        public double? SanimexSalesPricesPriceGAM01RetailPrice { get; set; }
        public double? SanimexSalesPricesPriceGAM01Tax { get; set; }
        public double? SanimexSalesPricesPriceGAM01WholesalePrice { get; set; }
        public double? SanimexSalesPricesPriceGAM02RetailPrice { get; set; }
        public double? SanimexSalesPricesPriceGAM02Tax { get; set; }
        public double? SanimexSalesPricesPriceGAM02WholesalePrice { get; set; }
        public double? SanimexSalesPricesPriceGAN01RetailPrice { get; set; }
        public double? SanimexSalesPricesPriceGAN01Tax { get; set; }
        public double? SanimexSalesPricesPriceGAN01WholesalePrice { get; set; }
        public double? SanimexSalesPricesPriceGAN02RetailPrice { get; set; }
        public double? SanimexSalesPricesPriceGAN02Tax { get; set; }
        public double? SanimexSalesPricesPriceGAN02WholesalePrice { get; set; }
        public double? SanimexSalesPricesDiscountPriceprecioDescuento { get; set; }
        public double? SanimexClassificationProductImported { get; set; }
        public string? SanimexSalesPricesShopDistribution { get; set; }
        public int? SanimexFeatureProductSquareMeter { get; set; }
        public string? SanimexFeatureProductPresentation { get; set; }
        public int? SanimexSalesResupply { get; set; }
    }
}
