#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Media3D;
using static Autodesk.Revit.DB.SpecTypeId;

#endregion

namespace RevitAddinBootCamp_Module01_Skills
{
    [Transaction(TransactionMode.Manual)]
    public class Module01Challenge : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;

            // ****** YOUR CODE GOES HERE ******.

            //1. Declare a number variable and set it to 250. 
            //2. Declare a starting elevation variable and set it to 0.
            //3. Declare a floor height variable and set it to 15.
            int possibleLevels = 250;
            double startingElevation = 0;
            int levelHeight = 15;

            // Filtered Element Collector (Get Titleblock using OfCategory() for TitleBlock with BuiltInCategory).
            FilteredElementCollector tbCollector = new FilteredElementCollector(doc);
            tbCollector.OfCategory(BuiltInCategory.OST_TitleBlocks);
            ElementId tblockId = tbCollector.FirstElementId();

            // Filtered Element Collector (Get view family types using OfClass()).    
            FilteredElementCollector vftCollector = new FilteredElementCollector(doc);
            vftCollector.OfClass(typeof(ViewFamilyType));

            ViewFamilyType fpVFT = null;
            ViewFamilyType cpVFT = null;


            foreach(ViewFamilyType curVFT in vftCollector)
            {
                if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                {
                    fpVFT = curVFT;
                }
                else if (curVFT.ViewFamily == ViewFamily.CeilingPlan)
                {
                    cpVFT = curVFT;
                }
            }

            /*When making a change in the Revit model, the code has to be after the Start() method, but before the Commit()
            method.  Create a floor level - show in Revit API (www.revitapidocs.com).
            Elevation value is in decimal feet regardless of a model's units.*/

            // Create a Transaction.
            Transaction tx = new Transaction(doc);
            tx.Start("Creating Levels in Revit");

            //4. For Loop to loop through the number 1 to the number variable.
            //*** Looping through possible levels 1 to 250. ***

            int fizzbuzzCount = 0;
            int fizzCount = 0;
            int buzzCount = 0;

            for (int i = 1; i <= possibleLevels; i++)
            {
                //5. Create a level for each number. 6. After creating the level, increment the current elevation by the floor height value.
                Level newLevel = Level.Create(doc, startingElevation);
                startingElevation += levelHeight;

                if (null == newLevel)
                {
                    throw new Exception("Create a new level failed.");

                }
                newLevel.Name = "LEVEL " + i.ToString();

                //9. If the number is divisible by both 3 and 5, create a sheet and name it "FIZZBUZZ_#".
                if (i % 3 == 0 && i % 5 == 0)
                {
                    //Create a sheet
                    ViewSheet newSheet = ViewSheet.Create(doc, tblockId);
                    newSheet.SheetNumber = "SHT-" + i.ToString();
                    newSheet.Name = "FIZZBUZZ-" + i.ToString();

                    fizzbuzzCount++;
                }
                else if (i % 3 == 0)
                {
                    //8. If the number is divisible by 5, create a ceiling plan and name it "BUZZ_#"/
                    ViewPlan newFloorPlan = ViewPlan.Create(doc, fpVFT.Id, newLevel.Id);
                    newFloorPlan.Name = "FIZZ-" + i.ToString();
                    fizzCount++;

                }

                else if (i % 5 == 0)
                {
                    //8. If the number is divisible by 5, create a ceiling plan and name it "BUZZ_#"/
                    ViewPlan newCeilingPlan = ViewPlan.Create(doc, cpVFT.Id, newLevel.Id);
                    newCeilingPlan.Name = "BUZZ-" + i.ToString();
                    buzzCount++;

                }

            }

            tx.Commit();
            tx.Dispose();


            TaskDialog.Show("Complete", $"Created {possibleLevels} levels.  Created {fizzbuzzCount} FIZZBUZZ Sheets.  Created {fizzCount}" +
                $" FIZZ floor plans. Created {buzzCount}" + "BUZZ ceiling plans");


            return Result.Succeeded;
        }

        public static System.String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}

