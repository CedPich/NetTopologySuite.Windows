using System.Windows;
using System.Windows.Media;
using Nts = NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Utilities;


namespace NetTopologySuite.Windows.Media
{
    
    ///<summary>
    /// Provides methods to read <see cref="Typeface"/> glyphs for strings 
    /// into <see cref="IPolygonal"/> geometry.
    ///</summary>
    /// <remarks>
    /// <para>
    /// It is suggested to use larger point sizes to render fonts glyphs, to reduce the effects of scale-dependent hints.</para>
    /// <para>The resulting geometry are in the base coordinate system of the font.</para>
    /// <para>The geometry can be further transformed as necessary using <see cref="AffineTransformation"/>s</para>
    /// </remarks>
    /// <author>Martin Davis</author>
    public static class FontGlyphReader
    {
        public const string FontSerif = "Serif";
        [System.Obsolete("Use SansSerif")]
        public const string FontSanserif = "SansSerif";
        public const string FontSansSerif = "SansSerif";
        public const string FontMonospaced = "Monospaced";

        // a flatness factor empirically determined to provide good results
        private const float FlatnessFactor = 400f;

        ///<summary>
        /// Converts text rendered in the given <see cref="Typeface"/> and pointsize to a <see cref="Nts.Geometry"/> using a standard flatness factor.
        /// </summary>
        /// <param name="text">The text to render</param>
        /// <param name="font">The <see cref="FontFamily"/></param>
        /// <param name="pointSize">The pointSize to render at</param>
        /// <param name="geomFact">The geometry factory to use to create the result</param>
        /// <returns>A polygonal geometry representing the rendered text</returns>
        public static Nts.Geometry Read(string text, FontFamily font, int pointSize, Nts.GeometryFactory geomFact)
        {
            return Read(text, font, FontStyles.Normal, pointSize, new System.Windows.Point(0,0),  geomFact);
        }

        ///<summary>
        /// Converts text rendered in the given <see cref="FontFamily"/> to a <see cref="Nts.Geometry"/> using a standard flatness factor.
        /// </summary>
        /// <param name="text">The text to render</param>
        /// <param name="font">The <see cref="FontFamily"/></param>
        /// <param name="geomFact">The geometry factory to use to create the result</param>
        /// <returns>A polygonal geometry representing the rendered text</returns>
        public static Nts.Geometry Read(string text, FontFamily font, Nts.GeometryFactory geomFact)
        {
            return Read(text, font, FontStyles.Normal, 12, new Point(0,0), geomFact);
        }

        public static Nts.Geometry Read(string text, FontFamily font, FontStyle style, float size, Point origin, Nts.GeometryFactory geomFact)

        {
            return Read(text, font, style, size, origin, FlowDirection.LeftToRight, size / FlatnessFactor, geomFact);
        }

        ///<summary>
        /// Converts text rendered in the given <see cref="FontFamily"/> and pointsize to a <see cref="Nts.Geometry"/> using a standard flatness factor.
        /// </summary>
        /// <param name="text">The text to render</param>
        /// <param name="font">The <see cref="FontFamily"/></param>
        /// <param name="size">The size to render at</param>
        /// <param name="style">The style to use</param>
        /// <param name="flatness">The flatness to use</param>
        /// <param name="origin">The point to start</param>
        /// <param name="flowDirection">The flow direction to use</param>
        /// <param name="geomFact">The geometry factory to use to create the result</param>
        /// <returns>A polygonal geometry representing the rendered text</returns>
        public static Nts.Geometry Read(string text, FontFamily font, FontStyle style, float size, Point origin, FlowDirection flowDirection, double flatness, Nts.GeometryFactory geomFact)
        {
            var typeFace = new Typeface(font, style, new FontWeight(), new FontStretch());
            return Read(text, typeFace, size, origin, flowDirection, geomFact);
        }

        public static Nts.Geometry Read(string text, Typeface font, double size, Point origin, FlowDirection flowDirection, Nts.GeometryFactory geomFact)
        {
            var formattedText = new FormattedText(text, System.Globalization.CultureInfo.CurrentUICulture,
                                                  flowDirection, font, size, Brushes.Black);

            var geom = formattedText.BuildGeometry(origin);
            return WpfGeometryReader.Read(geom.GetFlattenedPathGeometry(FlatnessFactor, ToleranceType.Relative), geomFact);
        }
        public static Nts.Geometry Read(string text, Typeface font, Nts.GeometryFactory geomFact)
        {
            return Read(text, font, 12, new Point(), FlowDirection.LeftToRight, geomFact);
        }
    }
}