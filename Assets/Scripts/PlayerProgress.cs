using UnityEngine;

namespace Geekbrains
{
    public class PlayerProgress : MonoBehaviour
    {
        private int _level = 1; // уровень персонажа
        private int _statPoints; // количество свободных очков характеристик для прокачки характеристик
        private int _skillPoints;
        private float _exp; // текущее количество опыта
        private float _nextLevelExp = 100; // опыт, необходимый для взятия следующего уровня

        private UserData data;

        public void Load(UserData data)
        {
            this.data = data;
            if (data.level > 0) _level = data.level;
            _statPoints = data.statPoints;
            _skillPoints = data.skillPoints;
            _exp = data.exp;
            if (data.nextLevelExp > 0) _nextLevelExp = data.nextLevelExp;
        }

        public void AddExp(float addExp)
        {
            data.exp = _exp += addExp;

            while (_exp >= _nextLevelExp)
            {
                data.exp = _exp -= _nextLevelExp;
                LevelUp();
            }

            if (_manager != null)
            {
                _manager.Exp = _exp;
                _manager.NextLevelExp = _nextLevelExp;
                _manager.Level = _level;
                _manager.StatPoints = _statPoints;
                _manager.skillPoints = _skillPoints;
            }
        }

        private void LevelUp()
        {
            data.level = ++_level;
            data.nextLevelExp = _nextLevelExp += 100f;
            data.statPoints = _statPoints += 3;
            data.skillPoints = _skillPoints += 1;
        }

        // менеджер характеристик
        private StatsManager _manager;

        // обновление синхронизируемых полей при установке менеджера
        public StatsManager Manager
        {
            set
            {
                _manager = value;
                _manager.Exp = _exp;
                _manager.NextLevelExp = _nextLevelExp;
                _manager.Level = _level;
                _manager.StatPoints = _statPoints;
                _manager.skillPoints = _skillPoints;
            }
        }

        public bool RemoveStatPoint()
        {
            if (_statPoints > 0)
            {
                data.statPoints = --_statPoints;
                if (_manager != null) _manager.StatPoints = _statPoints;
                return true;
            }
            return false;
        }

        public bool RemoveSkillPoint()
        {
            if (_skillPoints > 0)
            {
                data.skillPoints = --_skillPoints;
                if (_manager != null) _manager.skillPoints = _skillPoints;
                return true;
            }
            return false;
        }
    }
}