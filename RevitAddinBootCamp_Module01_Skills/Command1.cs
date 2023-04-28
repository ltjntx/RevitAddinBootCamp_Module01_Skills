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
    public class Command1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;

            // Your code goes here.
            // creat a comment using a double forward slash.
            // comments do not compile, so you can leave yourself notes in your code.
            
            // Let's create some variables.
            // DataType VariableName = Value; <- always end the line with a semicolon!

            // Create a string variable.
            string text1 = "This is my string text";
            string text2 = "This is my next string text";

            // Combine strings by concatenation.
            string text3 = text1 + text2;
            string text4 = text1 + " " + text2 + "abcd";

            // Create number variables.
            int number1 = 10;
            double number2 = 20.5;

            // Do some math.
            double number3 = number1 + number2;
            double number4 = number3 - number2;
            double number5 = number4 / number3;
            double number6 = number5 * number4;
            double number7 = (number6 + number5) / number4;

            // Convert meters to feet.
            double meters = 4;
            double metersToFeet = meters * 3.28084;

            // Convert mm to feet.
            double mm = 3500;
            double mmToFeet = mm / 304.8;
            double mmToFeet2 = (mm / 1000) * 3.28084;


            // Find the remainder when dividing (ie. the modulo or mod).
            //double remainder1 = 100 % 10; // equals 0 (100 divided by 10 = 10).
            //double remainder2 = 100 % 9; // equals 1 (100 divided by 9 = 11 with remainder of 1).

            // Increment a number by 1.
            number6++;
            number6 += 10;
            // Decrement a number by 1.
            number6--;

            // Use conditional logic to compare things.
            // compare using boolean operators
            // == equals
            // != not equal
            // > greater than
            // < less than
            // >= and <= 
            
            // Check a value and perform a single action if true.
            if (number6 > 10)
            {
                // do something if true.
            }

            // Check a value and perform an action if true and another if false.
            if (number5 == 100)
            {
                // Do something if true.
            }
            else
            {
                // Do something else if false.
            }

            // Check multiple values and perform actions if ture and false.
            if (number6 > 10)
            {
                // Do something if true.
            }
            else if (number6 == 8)
            {
                // Do something else if true.
            }
            else
            {
                //Do a third thing if false.
            }

            //Compound conditional statements.
            //look for two things (or more) using &&
            if (number6 > 10 && number5 == 100)
            {
                //Do something if both are true.
            }

            // Look for either thing using || (a logical or).
            if (number6 > 10 || number5 == 100)
            {
                //Do something if either is true.
            }

            // Create a list (List == Array?).
            List<string> list1 = new List<string>();

            // Add items to list.
            list1.Add(text1);
            list1.Add(text2);
            list1.Add("this is some text");

            //create list and add items to it - method 2
            List<int> list2 = new List<int> {1, 2, 3, 4, 5};

            // Loop through a list using foreach loop.
            int letterCounter = 0;
            foreach (string currentString in list1)
            {
                // Do something with currentString here.
                // letterCounter = letterCounter + currentString.Length;
                letterCounter += currentString.Length;
            }

            // For Loop, loops through a range of numbers.
            int numberCount = 0;
            int counter = 100;

            for (int i = 0; i <= counter; i++)
            {
                numberCount += i;
                
            }

            TaskDialog.Show("Number counter", "The number count is " + numberCount.ToString());

            //***Create a transaction to lock the model.***
            Transaction t = new Transaction(doc);
            t.Start("Doing something in Revit");

            /* Make a change in the Revit model.
               Create a floor level - show in Revit API (www.revitapidocs.com).
               Elevation value is in decimal feet regardless of model's units.*/
             double sElevation = 10;
             Level newLevel = Level.Create(doc, sElevation);
             newLevel.Name = "My new level";

            /*Setup for creating a floor plan view.
            create a floor plan view - show in Revit API (www.revitapidocs.com)
            by creating a Filtered Element Collector.*/
            FilteredElementCollector collector1 = new FilteredElementCollector(doc);
            collector1.OfClass(typeof(ViewFamilyType));

            ViewFamilyType floorPlanVFT = null;
            foreach (ViewFamilyType curVFT in collector1)
            {
                if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                { 
                    floorPlanVFT = curVFT;
                    break;
                }
            }

            // Create a view by specifying the document, view family type, and level.
            ViewPlan newPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
            newPlan.Name = "My new floor plan";

            //Get ceiling plan view family type.
            ViewFamilyType ceilingPlanVFT = null;
            foreach (ViewFamilyType curVFT in collector1)
            {
                if (curVFT.ViewFamily == ViewFamily.CeilingPlan)
                {
                    ceilingPlanVFT = curVFT;
                    break;
                }
            }

            // Create a ceiling plan using the ceiling plan view family type.
            ViewPlan newCeilingPlan = ViewPlan.Create(doc, ceilingPlanVFT.Id, newLevel.Id);
            newCeilingPlan.Name = "My new Ceiling plan";

            // Create a sheet (viewSheet).
            // but first I need to get a title block by creating a Filtered Element Collector.
            FilteredElementCollector collector2 = new FilteredElementCollector(doc);
            collector2.OfCategory(BuiltInCategory.OST_TitleBlocks);

            //Create a sheet.
            ViewSheet newSheet = ViewSheet.Create(doc, collector2.FirstElementId());
            newSheet.Name = "My New Sheet";
            newSheet.SheetNumber = "E-101";

            // Create a viewport and add a view to a sheet using a Viewport - show in API.
            // First create a point.
            //XYZ insertionPoint = new XYZ(1, 0.5, 0);
            //Viewport newViewport = Viewport.Create(doc, newSheet.Id, newPlan.Id, insertionPoint);


            t.Commit();
            t.Dispose();



            return Result.Succeeded;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}
