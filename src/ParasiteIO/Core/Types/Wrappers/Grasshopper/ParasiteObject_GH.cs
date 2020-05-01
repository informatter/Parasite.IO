
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using ParasiteIO.Core.Types;
using Grasshopper.Kernel;
using ParasiteIO.Core.Types.Geometry;

namespace ParasiteIO.Core.Types.Wrappers.Grasshopper
{
    public class ParasiteObject_GH : GH_GeometricGoo<ParasiteObject>, IGH_PreviewData
    {


        #region CONSTRUCTORS

        /// <summary>
        /// 
        /// </summary>
        public ParasiteObject_GH()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public ParasiteObject_GH(ParasiteObject value)
        {
            Value = value;

        }


        #endregion


        #region PROPERTIES



        public BoundingBox ClippingBox { get => Boundingbox; }


        public override ParasiteObject Value { get => base.Value; set => base.Value = value; }

        // <inheritdoc />
        public override bool IsValid
        {
            get
            {
                if (Value == null) { return false; }
                return true;
            }
        }

        public override bool IsReferencedGeometry => false;


        /// <inheritdoc />
        public override string TypeName { get => "ParasiteObject"; }



        /// <inheritdoc />
        public override string TypeDescription { get => "ParasiteObject"; }



        private BoundingBox _bbox = BoundingBox.Unset;
        /// <inheritdoc />
        public override BoundingBox Boundingbox
        {
            get
            {
                if (m_value != null && !_bbox.IsValid)
                    _bbox = new BoundingBox();

                return _bbox;
            }
        }

        #endregion;





        // <inheritdoc />
        public override IGH_GeometricGoo DuplicateGeometry() => new ParasiteObject_GH(new ParasiteObject(Value.Data));



        // / <inheritdoc />
        public override string ToString() => Value.ToString();



        /// <inheritdoc />
        public override object ScriptVariable() => Value;


        #region CAST METHODS
        // / <inheritdoc />
        public override bool CastTo<T>(ref T target)
        {
            if (typeof(T).IsAssignableFrom(typeof(ParasiteObject)))
            {
                object obj = Value;
                target = (T)obj;
                return true;
            }

            if (typeof(T).IsAssignableFrom(typeof(GH_ObjectWrapper)))
            {
                object obj = new GH_ObjectWrapper(Value);
                target = (T)obj;
                return true;
            }

            return false;
        }


        /// <inheritdoc />
        public override bool CastFrom(object source)
        {
            if (source is ParasiteObject ParasiteObject)
            {
                Value = ParasiteObject;
                return true;
            }


            return false;
        }

        #endregion

        #region TRANSFORM METHODS

        /// <inheritdoc />
        public override BoundingBox GetBoundingBox(Transform xform)
        {
            var b = Boundingbox;
            b.Transform(xform);
            return b;
        }


        /// / <inheritdoc />
        public override IGH_GeometricGoo Transform(Transform xform)
        {
            return null;

        }


        /// <inheritdoc />
        public override IGH_GeometricGoo Morph(SpaceMorph xmorph)
        {
            return null;
        }

        #endregion

        #region PREVIEW METHODS

        // IT SEEMS THAT IT DOES NOT NEED TO BE IMPLEMENTED HERE

        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            if (Value.Data == null) { return; }

            if (Value.Data is Parasite_BrepSolid)
            {

                Parasite_BrepSolid geo = Value.Data as Parasite_BrepSolid;

                Brep brep = ParasiteIO.Conversion.Rhinoceros.RhinoConversion.ToRhinoType(geo, 0.001);
                args.Pipeline.DrawBrepShaded(brep, args.Material);

            }
        }

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {

        }

        #endregion
    }
}
