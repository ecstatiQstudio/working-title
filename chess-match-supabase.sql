-- WARNING: This schema is for context only and is not meant to be run.
-- Table order and constraints may not be valid for execution.

CREATE TABLE public.game_state (
  id text NOT NULL,
  fen text NOT NULL,
  turn text NOT NULL DEFAULT 'w'::text,
  timer_ends_at timestamp with time zone,
  pgn text DEFAULT ''::text,
  CONSTRAINT game_state_pkey PRIMARY KEY (id)
);
CREATE TABLE public.move_votes (
  id uuid NOT NULL DEFAULT gen_random_uuid(),
  move_san text NOT NULL,
  created_at timestamp with time zone DEFAULT now(),
  player_session text,
  CONSTRAINT move_votes_pkey PRIMARY KEY (id)
);
CREATE TABLE public.move_votes_history (
  id uuid,
  move_san text,
  created_at timestamp with time zone,
  player_session text
);
CREATE TABLE public.game_state_history (
  id text,
  fen text,
  turn text,
  timer_ends_at timestamp with time zone,
  pgn text
);