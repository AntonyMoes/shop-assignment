using _Game.Scripts.Interaction;
using _Game.Scripts.Objects;
using _Game.Scripts.UI;
using UnityEngine;

namespace _Game.Scripts {
    public class Map : MonoBehaviour {
        [SerializeField] private Interactable[] _interactables;

        public void Init(IInteractPanelPresenter interactPanelPresenter, IEquipmentPanelPresenter equipmentPanelPresenter,
            IShopPanelPresenter shopPanelPresenter, InventoryObjectProvider provider) {
            foreach (var interactable in _interactables) {
                interactable.Init(interactPanelPresenter);

                switch (interactable) {
                    case Mirror mirror:
                        mirror.Setup(equipmentPanelPresenter);
                        break;
                    case Shop.ShopController shop:
                        shop.Setup(shopPanelPresenter, provider);
                        break;
                }
            }
        }
    }
}