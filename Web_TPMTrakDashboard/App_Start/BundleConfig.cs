using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace Web_TPMTrakDashboard
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254726
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                  "~/Scripts/WebForms/WebForms.js",
                  "~/Scripts/WebForms/WebUIValidation.js",
                  "~/Scripts/WebForms/MenuStandards.js",
                  "~/Scripts/WebForms/Focus.js",
                  "~/Scripts/WebForms/GridView.js",
                  "~/Scripts/WebForms/DetailsView.js",
                  "~/Scripts/WebForms/TreeView.js",
                  "~/Scripts/WebForms/WebParts.js"
                  
                  //,"~/AndonScripts/WebForms/WebForms.js",
                  //  "~/AndonScripts/WebForms/WebUIValidation.js",
                  //  "~/AndonScripts/WebForms/MenuStandards.js",
                  //  "~/AndonScripts/WebForms/Focus.js",
                  //  "~/AndonScripts/WebForms/GridView.js",
                  //  "~/AndonScripts/WebForms/DetailsView.js",
                  //  "~/AndonScripts/WebForms/TreeView.js",
                  //  "~/AndonScripts/WebForms/WebParts.js"                    
                    ));

            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"
                
                //,"~/AndonScripts/WebForms/MsAjax/MicrosoftAjax.js",
                //    "~/AndonScripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                //    "~/AndonScripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                //    "~/AndonScripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"
                    ));

            //ScriptManager.ScriptResourceMapping.AddDefinition(
            //   "respond",
            //   new ScriptResourceDefinition
            //   {
            //       Path = "~/AndonScripts/respond.min.js",
            //       DebugPath = "~/AndonScripts/respond.js",
            //   });
            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"
                //,"~/AndonScripts/modernizr-*"
                ));


            bundles.Add(new StyleBundle("~/bundles/logincss").Include(
                 "~/css/logincss/style.css",
                  "~/css/logincss/reset.css",
                    "~/css/logincss/weloveiconfonts.css"
                 ));
            bundles.Add(new ScriptBundle("~/bundles/loginjs").Include(
                "~/MyCssAndJS/toggel/jquery.min.js",
                 "~/MyCssAndJS/LoadingImgJs/JavaScriptUIBlocker.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/mastercss").Include(
                 "~/MyCssAndJS/bootstrap.min.css",
                  "~/css/elegant-icons-style.css",
                    "~/css/style.css",
                    "~/css/customer.css"
                //,
                //"~/MyCssAndJS/select/bootstrap-select.min.css",
                //"~/MyCssAndJS/css/bootstrap-datepicker.css"
                 ));
            bundles.Add(new ScriptBundle("~/bundles/masterjs").Include(
             "~/js/bootstrap.min.js",
              "~/js/jquery.scrollTo.min.js",
              //"~/js/jquery.nicescroll.js",
              "~/js/jquery.validate.min.js",
              "~/js/form-validation-script.js",
              "~/js/scripts.js",
              "~/MyCssAndJS/LoadingImgJs/JavaScriptUIBlocker.js"
             ));

            bundles.Add(new StyleBundle("~/bundles/datecss").Include(
               "~/Scripts/DateTimePicker/bootstrap-datepicker3.css"
           ));
            bundles.Add(new ScriptBundle("~/bundles/datejs").Include(
              "~/Scripts/DateTimePicker/bootstrap-datepicker.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/filtercss").Include(
                "~/MyCssAndJS/select/bootstrap-select.min.css"
            ));
            bundles.Add(new ScriptBundle("~/bundles/filterjs").Include(
              "~/MyCssAndJS/select/bootstrap-select.min.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/multiselectcss").Include(
              "~/Scripts/MultiCheckBox/bootstrap-multiselect.css"
          ));
            bundles.Add(new ScriptBundle("~/bundles/multiselectjs").Include(
              "~/Scripts/MultiCheckBox/bootstrap-multiselect.js"
            ));

            //bundles.Add(new StyleBundle("~/bundles/dashboardcss").Include(
            //    "~/MyCssAndJS/css/bootstrap-datepicker.css"
            //));
            bundles.Add(new ScriptBundle("~/bundles/dashboardjs").Include(
                "~/MyCssAndJS/toggel/jquery.min.js",
                "~/MyCssAndJS/toggel/bootstrap.min.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/dashboard2js").Include(
              "~/MyCssAndJS/js/jquery.js",
              "~/MyCssAndJS/js/bootstrap.min.js"
          ));
            bundles.Add(new StyleBundle("~/bundles/tableIconiccss").Include(
             "~/Content/Ionic.css"
         ));
            bundles.Add(new StyleBundle("~/bundles/dateTimecss").Include(
              "~/MyCssAndJS/DatePicker/bootstrap-datetimepicker.css"
          ));
            bundles.Add(new ScriptBundle("~/bundles/dateTimejs").Include(
                "~/MyCssAndJS/DatePicker/moment-with-locales.js",
                "~/MyCssAndJS/DatePicker/bootstrap-datetimepicker.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/VDGjs").Include(
               "~/MyCssAndJS/Charts/highstock.js",
               "~/MyCssAndJS/Charts/exporting.js"));

            bundles.Add(new ScriptBundle("~/bundles/drilldownChartjs").Include(
             "~/MyCssAndJS/chartjs/highcharts.js"
              ));
            bundles.Add(new ScriptBundle("~/bundles/paretoandDrillDownChartJs").Include(
                "~/MyCssAndJS/paretoAndDrillDown/highcharts.js",
             "~/MyCssAndJS/paretoAndDrillDown/data.js",
              "~/MyCssAndJS/paretoAndDrillDown/drilldown.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/paretoChartJs").Include(
                //"~/MyCssAndJS/pareto/highcharts.js",
                  "~/MyCssAndJS/pareto/pareto.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/commanChartjs").Include(
                "~/MyCssAndJS/chartjs/data.js",
                "~/MyCssAndJS/chartjs/drilldown.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/toastrJs").Include(
              "~/MyCssAndJS/tosterJs/toastr.min.js"
           ));
            bundles.Add(new StyleBundle("~/bundles/toastrCss").Include(
             "~/MyCssAndJS/tosterJs/toastr.min.css"
           ));

            bundles.Add(new ScriptBundle("~/bundles/editDropDownJs").Include(
             "~/MyCssAndJS/EditableDrop/jquery-editable-select.min.js"
          ));
            bundles.Add(new StyleBundle("~/bundles/editDropDownCss").Include(
             "~/MyCssAndJS/EditableDrop/jquery-editable-select.min.css"
           ));
            // bundles.Add(new StyleBundle("~/bundles/colorcss").Include(
            //  "~/Scripts/ColorPickerJs/css/pick-a-color-1.2.2.min.css"
            // ));
            // bundles.Add(new ScriptBundle("~/bundles/colorjs").Include(
            //    "~/Scripts/ColorPickerJs/dependencies/tinycolor-0.9.15.min.js",
            //    "~/Scripts/ColorPickerJs/js/pick-a-color-1.2.2.min.js"
            //));
            BundleTable.EnableOptimizations = true;
        }
    }
}