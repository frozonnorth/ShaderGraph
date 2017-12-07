using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Controls;
using UnityEngine;
using UnityEditor.Graphing;

namespace UnityEditor.ShaderGraph
{
    [Title("Input", "Texture", "Cubemap Asset")]
    public class CubemapAssetNode : AbstractMaterialNode, IPropertyFromNode
    {
        public const int OutputSlotId = 0;

        const string kOutputSlotName = "Out";

        public CubemapAssetNode()
        {
            name = "Cubemap Asset";
            UpdateNodeAfterDeserialization();
        }

        public sealed override void UpdateNodeAfterDeserialization()
        {
            AddSlot(new CubemapMaterialSlot(OutputSlotId, kOutputSlotName, kOutputSlotName, SlotType.Output));
            RemoveSlotsNameNotMatching(new[] { OutputSlotId });
        }

        [SerializeField]
        private SerializableCubemap m_Cubemap = new SerializableCubemap();

        [CubemapControl("")]
        public Cubemap cubemap
        {
            get { return m_Cubemap.cubemap; }
            set
            {
                if (m_Cubemap.cubemap == value)
                    return;
                m_Cubemap.cubemap = value;
                if (onModified != null)
                {
                    onModified(this, ModificationScope.Node);
                }
            }
        }

        public override void CollectShaderProperties(PropertyCollector properties, GenerationMode generationMode)
        {
            properties.AddShaderProperty(new CubemapShaderProperty()
            {
                overrideReferenceName = GetVariableNameForSlot(OutputSlotId),
                generatePropertyBlock = true,
                value = m_Cubemap,
                modifiable = false
            });
        }

        public override void CollectPreviewMaterialProperties(List<PreviewProperty> properties)
        {
            properties.Add(new PreviewProperty
            {
                name = GetVariableNameForSlot(OutputSlotId),
                propType = PropertyType.Cubemap,
                cubemapValue = cubemap
            });
        }

        public IShaderProperty AsShaderProperty()
        {
            var prop = new CubemapShaderProperty();
            prop.value = m_Cubemap;
            if (cubemap != null)
                prop.displayName = cubemap.name;
            return prop;
        }

        public int outputSlotId { get { return OutputSlotId; } }
    }
}