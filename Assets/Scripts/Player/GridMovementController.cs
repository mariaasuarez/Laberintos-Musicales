using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class GridMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float snapThreshold = 0.1f;
    
    [Header("Grid Settings")]
    private Vector2Int currentGridPosition;
    private Vector2Int targetGridPosition;
    private bool isMoving = false;
    
    [Header("References")]
    public Tilemap wallsTilemap;
    public Tilemap groundTilemap;
    
    private Vector3 targetWorldPosition;
    private Vector3 moveDirection;
    
    void Start()
    {
        // Inicializar posición en el grid
        currentGridPosition = WorldToGridPosition(transform.position);
        targetGridPosition = currentGridPosition;
        SnapToGrid();
    }
    
    void Update()
    {
        if (!isMoving)
        {
            HandleInput();
        }
        else
        {
            MoveToTarget();
        }
    }
    
    void HandleInput()
    {
        Vector2Int newDirection = Vector2Int.zero;
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            newDirection = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            newDirection = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            newDirection = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            newDirection = Vector2Int.right;
        
        if (newDirection != Vector2Int.zero)
        {
            Vector2Int potentialPosition = currentGridPosition + newDirection;
            
            if (CanMoveTo(potentialPosition))
            {
                targetGridPosition = potentialPosition;
                targetWorldPosition = GridToWorldPosition(targetGridPosition);
                moveDirection = (targetWorldPosition - transform.position).normalized;
                isMoving = true;
            }
        }
    }
    
    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, 
            targetWorldPosition, 
            moveSpeed * Time.deltaTime
        );
        
        // Verificar si llegó al destino
        if (Vector3.Distance(transform.position, targetWorldPosition) < snapThreshold)
        {
            transform.position = targetWorldPosition;
            currentGridPosition = targetGridPosition;
            isMoving = false;
            OnMovementComplete();
        }
    }
    
    bool CanMoveTo(Vector2Int gridPosition)
    {
        // Verificar si hay pared en la posición objetivo
        Vector3Int cellPosition = new Vector3Int(gridPosition.x, gridPosition.y, 0);
        
        if (wallsTilemap != null && wallsTilemap.HasTile(cellPosition))
            return false;
        
        // Verificar límites del grid
        if (gridPosition.x < 0 || gridPosition.y < 0)
            return false;
        
        return true;
    }
    
    Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPosition.x),
            Mathf.RoundToInt(worldPosition.y)
        );
    }
    
    Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x, gridPosition.y, 0);
    }
    
    void SnapToGrid()
    {
        transform.position = GridToWorldPosition(currentGridPosition);
    }
    
    void OnMovementComplete()
    {
        // Evento que se puede usar para otros sistemas
        Debug.Log($"Movement complete to cell: {currentGridPosition}");
    }
    
    // Para otros scripts que necesiten conocer la posición actual
    public Vector2Int GetCurrentGridPosition()
    {
        return currentGridPosition;
    }
    
    public bool IsPlayerMoving()
    {
        return isMoving;
    }
    void CheckForInstruments()
    {
        Vector3Int cellPosition = new Vector3Int(currentGridPosition.x, currentGridPosition.y, 0);

        // Verificar si hay un instrumento en la celda actual
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Instrument"))
            {
                InstrumentController instrument = collider.GetComponent<InstrumentController>();
                if (instrument != null)
                {
                    instrument.Interact();
                }
            }
        }
    }
}