using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField]
    private ConveyorSection sectionTemplate;

    [SerializeField, Range(.001f, 5f)]
    private float moveTickPeriodMsec = 0.1f;

    [SerializeField, Range(.001f, .1f)]
    private float moveTickDistance = .01f;

    [SerializeField]
    private float sectionIndent = 0.05f;

    // Высота секции с учетом промежутка
    private float FullSectionHeigh { get => sectionTemplate.Height + sectionIndent; }


    private SpriteRenderer _sr;
    private List<ConveyorSection> _sections = new();

    public float Height { get => _sr.bounds.size.y; }
    public float Width { get => _sr.bounds.size.x; }
    public float MaxY { get => _sr.bounds.max.y; }
    public float MinY { get => _sr.bounds.min.y; }
    public float MaxX { get => _sr.bounds.max.x; }
    public float MinX { get => _sr.bounds.min.x; }



    public Vector2 _spawnPoint;

    private IEnumerator Move()
    {

        while (_sections.Count > 0)
        {
            foreach (var section in _sections)
            {
                section.transform.position = new Vector3(section.transform.position.x, section.transform.position.y - moveTickDistance);
            }

            // При необходимости добавляем новую секцию
            if (_sections[0].MaxY < this.MaxY)
            {
                _sections.Insert(0, CreateSection(_spawnPoint));
            }

            // Удаляем секцию которая вышла за пределы конвеера
            var lastSectionIdx = _sections.Count - 1;
            if (_sections[lastSectionIdx].MaxY < this.MinY)
            {
                Debug.Log($"_sections.RemoveAt({lastSectionIdx})");
                Destroy(_sections[lastSectionIdx].gameObject);
                _sections.RemoveAt(lastSectionIdx);
            }

            yield return new WaitForSeconds(moveTickPeriodMsec);
        }
    }

    public void StartMove()
    {

        StartCoroutine(Move());
    }

    public void StopMove()
    {
        StopCoroutine(Move());
    }


    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();

        // Количество секций
        int sectionCount = (int)(Height / FullSectionHeigh);
        // Свободное место оставшееся после создания все секций
        float gap = Height - (sectionCount * FullSectionHeigh) - sectionIndent;
        // Верхний отступ
        float topMargin = (gap / 2) + (sectionTemplate.Height / 2);

        _spawnPoint = new Vector2(transform.position.x, MaxY + FullSectionHeigh - topMargin);

        for (int y = 0; y <= sectionCount; y++)
        {
            var section = CreateSection(new Vector2(_spawnPoint.x, _spawnPoint.y - (y * FullSectionHeigh)));

            //Instantiate(sectionTemplate, new Vector3(_spawnPoint.x, _spawnPoint.y - (y * FullSectionHeigh), 0), Quaternion.identity);
            
            _sections.Add(section);
        }

        StartMove();
    }

    private ConveyorSection CreateSection(Vector2 spawnPoin)
    {
        var section = Instantiate(sectionTemplate, spawnPoin, Quaternion.identity);
        section.transform.parent = this.gameObject.transform;
        return section;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
