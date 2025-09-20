ссылка на доску https://unidraw.io/app/board/a050c11d5d28fcccaa0e?allow_guest=true

## 1. Функциональные требования

1. Пользователь может зарегистрироваться и авторизоваться в системе.
2. Пользователь может создавать проекты и управлять ими.
3. Пользователь может создавать задачи внутри проекта, указывать исполнителя, приоритет и статус.
4. Пользователь может оставлять комментарии к задачам.
5. Пользователь может получать уведомления об изменении статуса или назначении на задачу.
6. Администратор может управлять пользователями (назначать роли: user, admin).
7. Сервис должен предоставлять REST API для интеграций.

## 2. Сервисы и зоны ответственности
- **Auth Service** — отвечает за регистрацию, вход, refresh токены, роли пользователей.
- **User Service** — управление профилями пользователей.
- **Project Service** — CRUD для проектов.
- **Task Service** — CRUD для задач, привязка к проектам и пользователям.
- **Comment Service** — хранение комментариев к задачам.
- **Notification Service** — уведомления (email, push, webhooks).

## Models
### **Task**
- id (guid, PK)
- project_id (FK → Project)
- title (string)
- description (text)
- status (enum: TODO, IN_PROGRESS, DONE)
-  priority (enum: LOW, MEDIUM, HIGH)
- assignee_id (FK → User)
- created_at (datetime)
- updated_at (datetime)
### **Project**
- id (guid, PK)
- name (string)

### **User**
- id (guid, PK)
- username (string)
- hashed_password (string)

REST эндпоинты Task Service
Base URL: /api/v1/tasks
POST /tasks — создать задачу
Request: { "title": "...", "description": "...", "project_id": "...", "priority": "HIGH" }
Response: 201 Created { "id": "...", ... }

GET /tasks/{task_id} — получить задачу по ID
Response: { "id": "...", "title": "...", "status": "TODO", ... }

GET /tasks?project_id=...&assignee_id=... — список задач с фильтрацией

PATCH /tasks/{task_id} — обновить (частично: статус, приоритет, описание, исполнитель)
Request: { "status": "IN_PROGRESS" }

DELETE /tasks/{task_id} — удалить задачу

POST /tasks/{task_id}/assign — назначить исполнителя
Request: { "assignee_id": "..." }
