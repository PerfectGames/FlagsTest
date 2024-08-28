using UnityEngine;

namespace FlagsTest
{
    public class TeamObjectWithRenderers :TeamObject
    {
        [SerializeField] Renderer[] _PaintableRenderers;

        public override void SetTeam (Team team)
        {
            base.SetTeam (team);

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            materialPropertyBlock.SetColor ("_Color", TeamColor);

            foreach (var renderer in _PaintableRenderers)
            {
                renderer.SetPropertyBlock (materialPropertyBlock);
            }
        }
    }
}
