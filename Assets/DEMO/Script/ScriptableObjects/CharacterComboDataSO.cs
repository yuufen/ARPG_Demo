using UnityEngine;

namespace DEMO.Combo {
    // 连招的单个招式
    [CreateAssetMenu(fileName = "ComboData", menuName = "Create/Character/ComboData", order = 0)]
    public class CharacterComboDataSO : ScriptableObject {
        [SerializeField] private string _comboName; // 招式名称
        [SerializeField] private string[] _comboHitName; // 对应的受击动画，因为一个 Clip 可能有多段攻击，所以这里是数组
        [SerializeField] private string[] _comboParryName; // 对应的格挡动画
        [SerializeField] private float _damage; // 伤害
        [SerializeField] private float _coldTime; // 衔接下一段攻击的间隔时间
        [SerializeField] private float _comboPositionOffset; // 这段招式角色与目标的距离，用来把主角吸附到敌人身边正确距离

        // Export
        public string ComboName => _comboName;
        public string[] ComboHitName => _comboHitName;
        public string[] ComboParryName => _comboParryName;
        public float ComboDamage => _damage;
        public float ComboColdTime => _coldTime;
        public float ComboPositionOffset => _comboPositionOffset;

        public int GetHitOrParryNameMaxCount() => _comboHitName.Length;
    }
}