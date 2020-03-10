using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Parasite.IO.Grasshopper
{
    public class ParasiteIOGrasshopperInfo : GH_AssemblyInfo
  {
    public override string Name
    {
        get
        {
            return "ParasiteIOGrasshopper";
        }
    }
    public override Bitmap Icon
    {
        get
        {
            //Return a 24x24 pixel bitmap to represent this GHA library.
            return null;
        }
    }
    public override string Description
    {
        get
        {
            //Return a short string describing the purpose of this GHA library.
            return "";
        }
    }
    public override Guid Id
    {
        get
        {
            return new Guid("57ae2d5c-7871-43e9-89ec-16adea2e09f1");
        }
    }

    public override string AuthorName
    {
        get
        {
            //Return a string identifying you or your company.
            return "";
        }
    }
    public override string AuthorContact
    {
        get
        {
            //Return a string representing your preferred contact details.
            return "";
        }
    }
}
}
