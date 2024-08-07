using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TrackingSystem
{

    public class CustomStepperStep : WebControl
    {
        public string Label { get; set; }
        public string Icon { get; set; }
        public bool Enabled { get; set; }
        public bool Selected { get; set; }
        public bool Error { get; set; }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "stepper-step");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "stepper-step-icon");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(Icon);
            writer.RenderEndTag(); // Div

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "stepper-step-label");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(Label);
            writer.RenderEndTag(); // Div

            writer.RenderEndTag(); // Div
        }
    }
}
