using Main.Lib.Mobs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.UI
{
    public class BossHealthController : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private TMP_Text bossNameText;
        private EnemyController _enemy;

        public void Setup(EnemyController enemy)
        {
            _enemy = enemy;
            bossNameText.text = enemy.name;
            healthSlider.maxValue = enemy.HealthComponent.MaxHealth;
            healthSlider.value = enemy.HealthComponent.Health;
            
            enemy.HealthComponent.OnHealthChange += OnBossHealthChange;
            enemy.HealthComponent.OnHealthZero += OnBossDie;
        }

        public void Enable()
        {
            healthSlider.gameObject.SetActive(true);
            bossNameText.gameObject.SetActive(true);
        }

        public void Disable()
        {
            healthSlider.gameObject.SetActive(false);
            bossNameText.gameObject.SetActive(false);
        }

        private void OnBossDie()
        {
            _enemy.HealthComponent.OnHealthChange -= OnBossHealthChange;
            _enemy.HealthComponent.OnHealthZero -= OnBossDie;
            _enemy = null;
            Disable();
        }

        private void OnBossHealthChange(float health)
        {
            healthSlider.value = health;
        }
    }
}
