using System.Collections.Generic;
using UnityEngine;

namespace DEMO.Combo {
    // 连招表
    [CreateAssetMenu(fileName = "Combo", menuName = "Create/Character/Combo", order = 0)]
    public class CharacterComboSO : ScriptableObject {
        // 连招招式列表
        [SerializeField] private List<CharacterComboDataSO> _allComboData = new List<CharacterComboDataSO>();

        // 封装一下

        // 获取连招数量
        public int TryGetComboMaxCount() => _allComboData.Count;

        // 获取招式段数
        public int TryGetHitOrParryMaxCount(int comboIndex) => _allComboData[comboIndex].GetHitOrParryNameMaxCount();


        // 获取招式
        public string TryGetOneComboAction(int comboIndex) {
            if (_allComboData.Count == 0) return null;

            return _allComboData[comboIndex].ComboName;
        }

        // 获取招式的属性
        public string TryGetHitName(int comboIndex, int hitIndex) {
            if (_allComboData.Count == 0) return null;
            if (_allComboData[comboIndex].GetHitOrParryNameMaxCount() == 0) return null;

            return _allComboData[comboIndex].ComboHitName[hitIndex];
        }

        public string TryGetParryName(int comboIndex, int hitIndex) {
            if (_allComboData.Count == 0) return null;
            if (_allComboData[comboIndex].GetHitOrParryNameMaxCount() == 0) return null;

            return _allComboData[comboIndex].ComboParryName[hitIndex];
        }

        public float TryGetComboDamage(int comboIndex) {
            if (_allComboData.Count == 0) return 0f;

            return _allComboData[comboIndex].ComboDamage;
        }

        public float TryGetComboColdTime(int comboIndex) {
            if (_allComboData.Count == 0) return 0f;

            return _allComboData[comboIndex].ComboColdTime;
        }

        public float TryGetComboPositionOffset(int comboIndex) {
            if (_allComboData.Count == 0) return 0f;

            return _allComboData[comboIndex].ComboPositionOffset;
        }
    }
}