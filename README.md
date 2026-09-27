# 🚀 Release Checklist Tool

A production-grade full-stack Release Checklist application built with **.NET 10**, **Hot Chocolate GraphQL**, **Entity Framework Core 10**, **PostgreSQL 17**, and an **Angular 19 (Standalone)** frontend using **Apollo Client**.

---

## 🏗️ Architecture & Key Design Decisions

1. **Pure Status Engine Logic (`ReleaseStepConfig.cs`)**
   - Release statuses (`PLANNED`, `ONGOING`, `DONE`) are derived pure functions computed dynamically based on the completion state of 7 fixed release steps.
   - Status is never manually modified or stored directly as mutable state by users; it is strictly computed when returning or updating releases.

2. **Database Optimization with JSONB (`completed_step_ids`)**
   - The step definitions are static/predefined across all releases, eliminating the need for a separate `steps` join table.
   - Completed step IDs are stored in a native PostgreSQL `JSONB` array (`completed_step_ids`). This avoids expensive SQL joins while maintaining schema flexibility and efficient querying.

3. **Hot Chocolate GraphQL API Layer**
   - Uses Hot Chocolate for high-performance GraphQL schema generation (`Query` and `Mutation`).
   - Replaces traditional REST endpoints with a single `/graphql` endpoint, eliminating over-fetching and under-fetching.

4. **Angular Standalone Architecture with Apollo Client**
   - Frontend is built using Angular Standalone Components without legacy `NgModule` boilerplate.
   - Leverages `apollo-angular` for reactive data fetching, caching, and seamless UI state updates on step toggles.

---

## 🗄️ Database Schema

### Table: `releases`

| Column | Type | Constraints | Description |
| :--- | :--- | :--- | :--- |
| `id` | `UUID` | Primary Key, Default `gen_random_uuid()` | Unique release identifier |
| `name` | `VARCHAR(200)` | NOT NULL | Name/title of the release |
| `release_date` | `TIMESTAMPTZ` | NOT NULL | Scheduled release timestamp |
| `additional_info` | `TEXT` | NULLABLE | Optional release notes/comments |
| `completed_step_ids` | `JSONB` | NOT NULL, Default `'[]'` | Array of completed step ID strings |
| `created_at` | `TIMESTAMPTZ` | NOT NULL, Default `NOW()` | Creation timestamp |
| `updated_at` | `TIMESTAMPTZ` | NOT NULL, Default `NOW()` | Last modification timestamp |

---

## 🔌 GraphQL API Operations Endpoint: `/graphql`

### Queries

#### Get Available Steps and Releases
```graphql
query GetReleasesAndSteps {
  availableSteps {
    id
    name
    description
    order
  }
  releases {
    id
    name
    releaseDate
    additionalInfo
    completedStepIds
    status
    createdAt
    updatedAt
  }
}