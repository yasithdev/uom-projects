#include<stdio.h>
#include <stdlib.h>


//node
typedef struct node {
    int val;
    struct node *next;
} node_t;

// Print
void print_list(node_t *head) {
    node_t *current = head;

    while (current != NULL) {
        printf("%d\n", current->val);
        current = current->next;
    }
}

// Member
int n_member(int val, node_t *head) {
    node_t *current = head;

    while (current != NULL) {
        if (current->val == val) {
            return 1;
        } else {
            current = current->next;
        }
    }
    return 0;
}

// Insert
int n_insert(int val, node_t **head) {
    node_t *current = *head;
    node_t *prev = NULL;

    while (current != NULL && current->val < val) {
        prev = current;
        current = current->next;
    }

    if (current == NULL || current->val > val) {
        node_t *temp = malloc(sizeof(node_t));
        temp->val = val;
        temp->next = current;
        if (prev == NULL) *head = temp;
        else prev->next = temp;
        return 1;
    } else {
        return 0;
    }
}

//delete
int n_delete(int val, node_t **head) {
    node_t *current = *head;
    node_t *pred = NULL;
    while (current != NULL && current->val < val) {
        pred = current;
        current = current->next;
    }
    if (current != NULL && current->val == val) {
        if (pred == NULL) {
            *head = current->next;
            free(current);
        } else {
            pred->next = current->next;
            free(current);
        }
        return 1;
    } else {
        return 0;
    }
}



