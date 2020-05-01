
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;


using Autodesk.Revit.UI;

namespace ParasiteIO.Revit.UI
{

    /// <summary>
    /// This class takes care of creating a Ribbon and adding elements to it
    /// </summary>
    public class ApplicationRibbonTab : IExternalApplication
    {
        #region METHODS

        #region IMPLICIT INTERFACE IMPLEMENTATION

        /// <summary>
        /// Adds ribbon when Revit Starts up.
        /// 
        /// </summary>
        /// <param name="a">A Revit User Interface object</param>
        /// <returns></returns>
        public Result OnStartup(UIControlledApplication a)
        {


            AddRibbonTab(a);
            return Result.Succeeded;
        }




        #endregion

        /// <summary>
        /// Creates a Ribbon Tab and Buttons
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public RibbonPanel AddRibbonTab(UIControlledApplication application)
        {


            // Create a custom ribbon tab
            String tabName = "FOJAB";
            application.CreateRibbonTab(tabName);

            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "Parasite.IO ");

            // Get dll assembly path via reflection.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // create data for push button 
            PushButtonData b1Data = new PushButtonData("Parasite.IO", "Parasite.IO", thisAssemblyPath, "FOJABcode_Revit.Commands.Command_AreasToVolumes");

   

            // create push button
            PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;



            // create button description
            pb1.ToolTip = "Create Volumes from Area objects";

     

            // create file path for application image
            var globePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "ApplicationIcon.png");

            BitmapImage pb1Image = new BitmapImage(new Uri(globePath));
            pb1.LargeImage = pb1Image;



            return ribbonPanel;
        }


        /// <summary>
        /// Performs operations when Revit is shutting down
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

        #endregion  
    }
}


