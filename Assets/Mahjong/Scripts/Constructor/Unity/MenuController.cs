using Scellecs.Morpeh;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DDX
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private RectTransform _figuresListContainer;
        [SerializeField] private Button _figurePrefab;
        [SerializeField] private GridMono _grid;
        [SerializeField] private GameObject _dicePrefab;
        [SerializeField] private RectTransform _dicesContainer;

        private string _gridName;
        private List<Button> figureItems = new List<Button>();
        private Figure[] _figures;

        private void Awake()
        {
            UpdateFiguresList();
        }

        #region UI Callback's
        public void SaveGrid()
        {
            var positions = _grid.Entity.GetComponent<GridPositionsList>().ConstructorPlacedPositions.Select(x => x.Value.Position);

            if(positions.Count() %3 != 0)
            {
                Debug.LogError($"Кол-во дайсов должно быть кратно трем. Сейчас дайсов: {positions.Count()}");
                return;
            }

            var figure = new Figure();
            figure.Name = _gridName;
            figure.Points = positions.ToList();

            AssetDatabase.CreateAsset(figure, $"Assets/Mahjong/Resources/Figures/{figure.Name}.asset");

            UpdateFiguresList();
        }
        public void SetGridName(string gridName)
        {
            _gridName = gridName;
        }
        public void ClearGrid()
        {
            GetComponent<EntityRefMono>().Entity.AddComponent<ClearGridEvent>();
        }
        private void LoadGrid(Figure figure)
        {
            GetComponent<EntityRefMono>().Entity.AddComponent<ClearGridEvent>();

            ref var @event = ref GetComponent<EntityRefMono>().Entity.AddComponent<InstantiateFigureEvent>();
            @event.Figure = figure;
            @event.DicesContainer = _dicesContainer;
            @event.DicePrefab = _dicePrefab;
        }
        #endregion

        private void UpdateFiguresList()
        {
            foreach (var item in figureItems)
                Destroy(item.gameObject);
            _figures = Resources.LoadAll<Figure>("Figures/");
            foreach (var item in _figures)
            {
                var btn = Instantiate(_figurePrefab, _figuresListContainer);
                btn.GetComponentInChildren<TMP_Text>().text = item.Name;
                btn.onClick.AddListener(() => OnFigureSelected(item));
                figureItems.Add(btn);
            }

        }

        private void OnFigureSelected(Figure item)
        {
            LoadGrid(item);
        }

        private void GenerateRandomGrid()
        {
            GetComponent<EntityRefMono>().Entity.AddComponent<RequestGenerateFigureEvent>();
        }


    }
}
