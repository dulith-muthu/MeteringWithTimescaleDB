-- postgreSQL + TimescaleDB (DB Clean up)
SELECT remove_continuous_aggregate_policy('"ApiUsageSummaryMinutely"', if_exists => TRUE);

SELECT remove_retention_policy('"ApiUsages"', if_exists => TRUE);
DROP INDEX IF EXISTS _timescaledb_internal.ix_context_identifier_time_minutely;
DROP MATERIALIZED VIEW IF EXISTS "ApiUsageSummaryMinutely";

DROP INDEX IF EXISTS ix_context_identifier_time;
DROP TABLE IF EXISTS "ApiUsages";
