#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace RevitAddinBootCamp_Module01_Skills
{
    [Transaction(TransactionMode.Manual)]
    public class Module01ChallengeAnswer : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;

            // *** My code goes here ***.

            // 1. set variables.
            int numFloors = 250;
            double currentElev = 0;
            int floorHeight = 15;

            // 7. Filtered Element Collector (Get Titleblock using OfCategory() for TitleBlock with BuiltInCategory).
            FilteredElementCollector tbCollector = new FilteredElementCollector(doc);
            tbCollector.OfCategory(BuiltInCategory.OST_TitleBlocks);
            ElementId tblockId = tbCollector.FirstElementId();

            // 8. Filtered Element Collector (Get view family types using OfClass()).    
            FilteredElementCollector vftCollector = new FilteredElementCollector(doc);
            vftCollector.OfClass(typeof(ViewFamilyType));

            // Declaring variables for VFT and setting the value to null.
            ViewFamilyType fpVFT = null;
            ViewFamilyType cpVFT = null;


            foreach (ViewFamilyType curVFT in vftCollector)
            {
                if(curVFT.ViewFamily == ViewFamily.FloorPlan)
                {
                    fpVFT = curVFT;

                }
                else if(curVFT.ViewFamily == ViewFamily.CeilingPlan)
                {
                    cpVFT = curVFT;
                }
            }

        // 9. create transaction.
        Transaction tx = new Transaction(doc);
        tx.Start("Fizz Buzz Challenge");

        int fizzbuzzCounter = 0;
        int fizzCounter = 0;
        int buzzCounter = 0;


        // 4. LOOP THROUGH FLOORS AND CHECK FIZZBUZZ.
        for (int i = 1; i <= numFloors; i++)
        {
            // 5. create level (inside the for loop).
            Level newLevel = Level.Create(doc, currentElev);
            newLevel.Name = "LEVEL " + i.ToString();
            currentElev += floorHeight;

            // 7., 8., & 9. check for FIZZBUZZ.

            if (i % 3 == 0 && i % 5 == 0)
            {
                //FIZZBUZZ = Create a sheet.
                ViewSheet newSheet = ViewSheet.Create(doc, tblockId);
                newSheet.SheetNumber = "RAB-" + i.ToString();
                newSheet.Name = "FIZZBUZZ-" + i.ToString();

                //BONUS
                ViewPlan newFloorPlan = ViewPlan.Create(doc, fpVFT.Id, newLevel.Id);
                newFloorPlan.Name = "FIZZBUZZ-" + i.ToString();

                XYZ insPoint = new XYZ(1.5, 1, 0);
                Viewport newVP = Viewport.Create(doc, newSheet.Id, newFloorPlan.Id, insPoint);

                fizzbuzzCounter++;

            }
            else if (i % 3 == 0)
            {

                // FIZZ = Create a floor plan.
                ViewPlan newFloorPlan = ViewPlan.Create(doc, fpVFT.Id, newLevel.Id);
                newFloorPlan.Name = "FIZZ-" + i.ToString();
                fizzCounter++;


            }
            else if (i % 5 == 0)
            {
                // BUZZ = Create a ceiling plan.
                ViewPlan newCeilingPlan = ViewPlan.Create(doc, cpVFT.Id, newLevel.Id);
                newCeilingPlan.Name = "BUZZ-" + i.ToString();
                buzzCounter++;

            }
        }

        tx.Commit();
        tx.Dispose();

        // 11. Create an Alert to the user.
        TaskDialog.Show("Complete", $"Created {numFloors} levels. Created {fizzbuzzCounter} FIZZBUZZ sheets. Created {fizzCounter} FIZZ floor plans. "
            + $"Created {buzzCounter} BUZZ ceiling plans.");

        return Result.Succeeded;
        }    
    }
}
