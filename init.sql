-- Create table for storing release checklists
CREATE TABLE IF NOT EXISTS releases (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    release_date TIMESTAMPTZ NOT NULL,
    additional_info TEXT NULL,
    completed_step_ids JSONB NOT NULL DEFAULT '[]'::jsonb,
    created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Index to quickly sort releases by date
CREATE INDEX IF NOT EXISTS idx_releases_release_date ON releases (release_date DESC);