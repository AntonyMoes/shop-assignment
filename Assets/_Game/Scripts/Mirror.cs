using _Game.Scripts.Interaction;
using _Game.Scripts.UI;
using UnityEngine;

namespace _Game.Scripts {
    public class Mirror : Interactable {
        private IEquipmentPanelPresenter _equipmentPanelPresenter;

        public void Setup(IEquipmentPanelPresenter equipmentPanelPresenter) {
            _equipmentPanelPresenter = equipmentPanelPresenter;
        }

        public override void Interact() {
            _equipmentPanelPresenter.ShowEquipmentPanel();
        }
    }
}