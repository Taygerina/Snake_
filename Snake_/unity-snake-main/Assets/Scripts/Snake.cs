using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    //Variaveis 
    private List<Transform> _segments = new List<Transform>();

    public Transform segmentPrefab;

    public Vector2 direction = Vector2.right;

    public int initialSize = 4;

    //Jogo inicia
    private void Start()
    {

        ResetState();
    }

    //Definindo o controle
    private void Update()
    {
        //Se apertar S ele vai prar baixo e se apertar W ele vai para cima.
        if (this.direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                this.direction = Vector2.up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                this.direction = Vector2.down;
            }
        }
        //Se apertar D vai para direita e se for para A para esquerda.
        else if (this.direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                this.direction = Vector2.right;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                this.direction = Vector2.left;
            }
        }
    }

    private void FixedUpdate()
    {
        // Definindo  o gameobj corpo
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        // Atualizando sempre o x e o y do corpo
        float x = Mathf.Round(this.transform.position.x) + this.direction.x;
        float y = Mathf.Round(this.transform.position.y) + this.direction.y;

        this.transform.position = new Vector2(x, y);
    }

    //Definindo a coletavel.
    public void Grow()
    {
        //quando comer a comida adiciona um novo gameobj.
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
    }

    //reseta o jogo quando morrer
    public void ResetState()
    {

        this.direction = Vector2.right;
        this.transform.position = Vector3.zero;


        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }


        _segments.Clear();
        _segments.Add(this.transform);


        for (int i = 0; i < this.initialSize - 1; i++)
        {
            Grow();
        }
    }

    //Definindo o collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se colidir com a comida coleta.
        if (other.tag == "Food")
        {
            Grow();
        }
        //Quando colidir com a parede, reseta o jogo.
        else if (other.tag == "Obstacle")
        {
            ResetState();
        }
    }

}
