using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
    public struct FObject
    {
        [IgnoreDataMember] public SyncData Data { get; set; }

        [DataMember(Name = "id")] public string Id { get; set; }
        [DataMember(Name = "name")] public string Name { get; set; }
        [DataMember(Name = "type")] public string Type { get; set; }
        //[DataMember(Name = "scrollBehavior")] public string ScrollBehavior { get; set; }
        [DataMember(Name = "blendMode")] public string BlendMode { get; set; }
        [DataMember(Name = "children")] public List<FObject> Children { get; set; }
        [DataMember(Name = "absoluteBoundingBox")] public BoundingBox AbsoluteBoundingBox { get; set; }
        [DataMember(Name = "absoluteRenderBounds")] public BoundingBox AbsoluteRenderBounds { get; set; }
        [DataMember(Name = "constraints")] public Constraints Constraints { get; set; }
        [DataMember(Name = "relativeTransform")] public List<List<float?>> RelativeTransform { get; set; }
        [DataMember(Name = "size")] public Vector2 Size { get; set; }
        [DataMember(Name = "clipsContent")] public bool? ClipsContent { get; set; }
        //[DataMember(Name = "background")] public List<Paint> Background { get; set; }
        [DataMember(Name = "fills")] public List<Paint> Fills { get; set; }
        [DataMember(Name = "strokes")] public List<Paint> Strokes { get; set; }
        [DataMember(Name = "cornerRadius")] public float? CornerRadius { get; set; }
        [DataMember(Name = "strokeWeight")] public float StrokeWeight { get; set; }
        [DataMember(Name = "strokeAlign")] public string StrokeAlign { get; set; }
        //[DataMember(Name = "backgroundColor")] public Color BackgroundColor { get; set; }
        [DataMember(Name = "effects")] public List<Effect> Effects { get; set; }
        //[DataMember(Name = "prototypeStartNodeID")] public string PrototypeStartNodeID { get; set; }
        //[DataMember(Name = "flowStartingPoints")] public List<FlowStartingPoint> FlowStartingPoints { get; set; }
        //[DataMember(Name = "prototypeDevice")] public PrototypeDevice PrototypeDevice { get; set; }
        [DataMember(Name = "fillGeometry")] public List<FillGeometry> FillGeometry { get; set; }
        [DataMember(Name = "strokeGeometry")] public List<FillGeometry> StrokeGeometry { get; set; }
        //[DataMember(Name = "booleanOperation")] public string BooleanOperation { get; set; }
        [DataMember(Name = "strokeCap")] public string StrokeCap { get; set; }
        [DataMember(Name = "strokeJoin")] public string StrokeJoin { get; set; }
        //[DataMember(Name = "styles")] public Styles Styles { get; set; }
        [DataMember(Name = "strokeMiterAngle")] public float? StrokeMiterAngle { get; set; }
        [DataMember(Name = "opacity")] public float? Opacity { get; set; }
        //[DataMember(Name = "preserveRatio")] public bool? PreserveRatio { get; set; }
        [DataMember(Name = "layoutAlign")] public string LayoutAlign { get; set; }
        [DataMember(Name = "layoutGrow")] public float? LayoutGrow { get; set; }
        [DataMember(Name = "characters")] public string Characters { get; set; }
        [DataMember(Name = "style")] public Style Style { get; set; }
        //[DataMember(Name = "layoutVersion")] public int? LayoutVersion { get; set; }
        //[DataMember(Name = "characterStyleOverrides")] public List<object> CharacterStyleOverrides { get; set; }
        [DataMember(Name = "styleOverrideTable")] public Dictionary<string, Style> StyleOverrideTable { get; set; }
        //[DataMember(Name = "lineTypes")] public List<string> LineTypes { get; set; }
        //[DataMember(Name = "lineIndentations")] public List<int> LineIndentations { get; set; }
        [DataMember(Name = "layoutMode")] public string LayoutMode { get; set; }
        [DataMember(Name = "itemSpacing")] public float? ItemSpacing { get; set; }
        [DataMember(Name = "counterAxisSpacing")] public float? CounterAxisSpacing { get; set; }
        //[DataMember(Name = "componentId")] public string ComponentId { get; set; }
        [DataMember(Name = "visible")] public bool? Visible { get; set; }
        [DataMember(Name = "primaryAxisSizingMode")] public string PrimaryAxisSizingMode { get; set; }
        [DataMember(Name = "counterAxisSizingMode")] public string CounterAxisSizingMode { get; set; }//
        [DataMember(Name = "counterAxisAlignContent")] public string CounterAxisAlignContent { get; set; }
        [DataMember(Name = "primaryAxisAlignItems")] public string PrimaryAxisAlignItems { get; set; }
        [DataMember(Name = "counterAxisAlignItems")] public string CounterAxisAlignItems { get; set; }
        [DataMember(Name = "layoutWrap")] public string LayoutWrap { get; set; }
        [DataMember(Name = "isMask")] public bool? IsMask { get; set; }
        [DataMember(Name = "paddingLeft")] public float? PaddingLeft { get; set; }
        [DataMember(Name = "paddingRight")] public float? PaddingRight { get; set; }
        [DataMember(Name = "paddingTop")] public float? PaddingTop { get; set; }
        [DataMember(Name = "paddingBottom")] public float? PaddingBottom { get; set; }
        [DataMember(Name = "horizontalPadding")] public float? HorizontalPadding { get; set; }
        [DataMember(Name = "verticalPadding")] public float? VerticalPadding { get; set; }
        //[DataMember(Name = "fillOverrideTable")] public object FillOverrideTable { get; set; }
        [DataMember(Name = "rectangleCornerRadii")] public List<float> CornerRadiuses { get; set; }
        [DataMember(Name = "arcData")] public ArcData ArcData { get; set; }
        [DataMember(Name = "strokeDashes")] public List<float> StrokeDashes { get; set; }
        //[DataMember(Name = "exportSettings")] public List<ExportSetting> ExportSettings { get; set; }
        //[DataMember(Name = "locked")] public bool? Locked { get; set; }
        //[DataMember(Name = "isFixed")] public bool? IsFixed { get; set; }
        //[DataMember(Name = "layoutGrids")] public List<LayoutGrid> LayoutGrids { get; set; }
        [DataMember(Name = "layoutPositioning")] public string LayoutPositioning { get; set; }
        //[DataMember(Name = "isMaskOutline")] public bool? IsMaskOutline { get; set; }
        //[DataMember(Name = "individualStrokeWeights")] public IndividualStrokeWeights IndividualStrokeWeights { get; set; }
        //[DataMember(Name = "transitionNodeID")] public string TransitionNodeID { get; set; }
        //[DataMember(Name = "transitionDuration")] public float? TransitionDuration { get; set; }
        //[DataMember(Name = "transitionEasing")] public string TransitionEasing { get; set; }
    }

    public struct FigmaComponent
    {
        [DataMember(Name = "key")] public string Key { get; set; }
        [DataMember(Name = "name")] public string Name { get; set; }
        //[DataMember(Name = "description")] public string Description { get; set; }
        //[DataMember(Name = "remote")] public bool Remote { get; set; }
        //[DataMember(Name = "documentationLinks")] public List<object> DocumentationLinks { get; set; }
    }

    public struct FigmaProject
    {
        [DataMember(Name = "document")] public FObject Document { get; set; }
        //[DataMember(Name = "schemaVersion")] public int SchemaVersion { get; set; }
        [DataMember(Name = "name")] public string Name { get; set; }
        //[DataMember(Name = "lastModified")] public string LastModified { get; set; }
        //[DataMember(Name = "thumbnailUrl")] public string ThumbnailUrl { get; set; }
        //[DataMember(Name = "version")] public string Version { get; set; }
        //[DataMember(Name = "role")] public string Role { get; set; }
        //[DataMember(Name = "editorType")] public string EditorType { get; set; }
        //[DataMember(Name = "linkAccess")] public string LinkAccess { get; set; }
        [DataMember(Name = "components")] public Dictionary<string, FigmaComponent> Components { get; set; }
        //[DataMember(Name = "componentSets")] public object ComponentSets { get; set; }
        //[DataMember(Name = "styles")] public object Styles { get; set; }
    }

    public struct Constraints
    {
        [DataMember(Name = "vertical")] public string Vertical { get; set; }
        [DataMember(Name = "horizontal")] public string Horizontal { get; set; }
    }

    // [Serializable]
    /// <summary>
    /// A solid color, gradient, or image texture that can be applied as fills or strokes
    /// </summary>
    public struct Paint
    {
        [DataMember(Name = "blendMode")] public string BlendMode { get; set; }
        [DataMember(Name = "type")] public string Type { get; set; }
        [DataMember(Name = "color")] public Color Color { get; set; }
        [DataMember(Name = "visible")] public bool? Visible { get; set; }
        [DataMember(Name = "scaleMode")] public string ScaleMode { get; set; }
        [DataMember(Name = "scalingFactor")] public string ScalingFactor { get; set; }
        [DataMember(Name = "imageRef")] public string ImageRef { get; set; }
        [DataMember(Name = "gifRef")] public string GifRef { get; set; }
        [DataMember(Name = "imageTransform")] public List<List<float>> ImageTransform { get; set; }
        [DataMember(Name = "gradientHandlePositions")] public List<Vector2> GradientHandlePositions { get; set; }
        [DataMember(Name = "gradientStops")] public List<GradientStop> GradientStops { get; set; }
        [DataMember(Name = "opacity")] public float? Opacity { get; set; }
        [DataMember(Name = "filters")] public Filters Filters { get; set; }
        [DataMember(Name = "rotation")] public float? Rotation { get; set; }
    }

    //[Serializable]
    /// <summary>
    /// A visual effect such as a shadow or blur.
    /// </summary>
    public struct Effect
    {
        [DataMember(Name = "type")] public string Type { get; set; }
        [DataMember(Name = "visible")] public bool? Visible { get; set; }
        [DataMember(Name = "color")] public Color Color { get; set; }
        [DataMember(Name = "opacity")] public float? Opacity { get; set; }
        [DataMember(Name = "blendMode")] public string BlendMode { get; set; }
        [DataMember(Name = "offset")] public Vector2 Offset { get; set; }
        [DataMember(Name = "radius")] public float Radius { get; set; }
        [DataMember(Name = "showShadowBehindNode")] public bool? ShowShadowBehindNode { get; set; }
        [DataMember(Name = "spread")] public float? Spread { get; set; }
    }

    public struct Style
    {
        [DataMember(Name = "fontFamily")] public string FontFamily { get; set; }
        [DataMember(Name = "fontPostScriptName")] public string FontPostScriptName { get; set; }
        [DataMember(Name = "fontWeight")] public int FontWeight { get; set; }
        [DataMember(Name = "textAutoResize")] public string TextAutoResize { get; set; }
        [DataMember(Name = "fontSize")] public float FontSize { get; set; }
        [DataMember(Name = "textAlignHorizontal")] public string TextAlignHorizontal { get; set; }
        [DataMember(Name = "textAlignVertical")] public string TextAlignVertical { get; set; }
        [DataMember(Name = "letterSpacing")] public float LetterSpacing { get; set; }
        [DataMember(Name = "lineHeightPx")] public float LineHeightPx { get; set; }
        //[DataMember(Name = "lineHeightPercent")] public float LineHeightPercent { get; set; }
        //[DataMember(Name = "lineHeightUnit")] public string LineHeightUnit { get; set; }
        //[DataMember(Name = "hyperlink")] public Hyperlink Hyperlink { get; set; }
        //[DataMember(Name = "lineHeightPercentFontSize")] public float? LineHeightPercentFontSize { get; set; }
        [DataMember(Name = "textCase")] public string TextCase { get; set; }
        [DataMember(Name = "textDecoration")] public string TextDecoration { get; set; }
        //[DataMember(Name = "opentypeFlags")] public OpentypeFlags OpentypeFlags { get; set; }
        [DataMember(Name = "italic")] public bool? Italic { get; set; }
    }

    public struct Filters
    {
        [DataMember(Name = "exposure")] public float? Exposure { get; set; }
        [DataMember(Name = "contrast")] public float? Contrast { get; set; }
        [DataMember(Name = "saturation")] public float? Saturation { get; set; }
        [DataMember(Name = "temperature")] public float? Temperature { get; set; }
        [DataMember(Name = "tint")] public float? Tint { get; set; }
        [DataMember(Name = "highlights")] public float? Highlights { get; set; }
        [DataMember(Name = "shadows")] public float? Shadows { get; set; }
    }

    public struct GradientStop
    {
        [DataMember(Name = "color")] public Color Color { get; set; }
        [DataMember(Name = "position")] public float Position { get; set; }
    }

    public struct FillGeometry
    {
        [DataMember(Name = "path")] public string Path { get; set; }
        //[DataMember(Name = "windingRule")] public string WindingRule { get; set; }
        //[DataMember(Name = "overrideID")] public int? OverrideID { get; set; }
    }

    // [Serializable]
    /// <summary>
    /// Information about the arc properties of an ellipse. 0° is the x axis and increasing angles rotate clockwise.
    /// </summary>
    public struct ArcData
    {
        [DataMember(Name = "startingAngle")] public float StartingAngle { get; set; }
        [DataMember(Name = "endingAngle")] public float EndingAngle { get; set; }
        [DataMember(Name = "innerRadius")] public float InnerRadius { get; set; }
    }

    public struct BoundingBox
    {
        [DataMember(Name = "x")] public float? X { get; set; }
        [DataMember(Name = "y")] public float? Y { get; set; }
        [DataMember(Name = "width")] public float? Width { get; set; }
        [DataMember(Name = "height")] public float? Height { get; set; }
    }

    public struct Hyperlink
    {
        [DataMember(Name = "type")] public string Type { get; set; }
        [DataMember(Name = "url")] public string Url { get; set; }
    }

    public struct Styles
    {
        [DataMember(Name = "stroke")] public string Stroke { get; set; }
        [DataMember(Name = "fill")] public string Fill { get; set; }
        [DataMember(Name = "fills")] public string Fills { get; set; }
        [DataMember(Name = "text")] public string Text { get; set; }
        [DataMember(Name = "effect")] public string Effect { get; set; }
        [DataMember(Name = "strokes")] public string Strokes { get; set; }
        [DataMember(Name = "grid")] public string Grid { get; set; }
    }

    public struct Constraint
    {
        [DataMember(Name = "type")] public string Type { get; set; }
        [DataMember(Name = "value")] public float Value { get; set; }
    }

    public struct ExportSetting
    {
        [DataMember(Name = "suffix")] public string Suffix { get; set; }
        [DataMember(Name = "format")] public string Format { get; set; }
        [DataMember(Name = "constraint")] public Constraint Constraint { get; set; }
    }

    public struct LayoutGrid
    {
        [DataMember(Name = "pattern")] public string Pattern { get; set; }
        [DataMember(Name = "sectionSize")] public float SectionSize { get; set; }
        [DataMember(Name = "visible")] public bool Visible { get; set; }
        [DataMember(Name = "color")] public Color Color { get; set; }
        [DataMember(Name = "alignment")] public string Alignment { get; set; }
        [DataMember(Name = "gutterSize")] public float GutterSize { get; set; }
        [DataMember(Name = "offset")] public float Offset { get; set; }
        [DataMember(Name = "count")] public int Count { get; set; }
    }

    public struct OpentypeFlags
    {
        [DataMember(Name = "CASE")] public int Case { get; set; }
    }

    public struct IndividualStrokeWeights
    {
        [DataMember(Name = "top")] public float Top { get; set; }
        [DataMember(Name = "right")] public float Right { get; set; }
        [DataMember(Name = "bottom")] public float Bottom { get; set; }
        [DataMember(Name = "left")] public float Left { get; set; }
    }

    public struct FlowStartingPoint
    {
        [DataMember(Name = "nodeId")] public string NodeId { get; set; }
        [DataMember(Name = "name")] public string Name { get; set; }
    }

    public struct PrototypeDevice
    {
        [DataMember(Name = "type")] public string Type { get; set; }
        [DataMember(Name = "rotation")] public string Rotation { get; set; }
    }
}