 -- postgreSQL + TimescaleDB (Initialize DB)
 
 -- create raw data table, hypertable and index
 CREATE TABLE "ApiUsages" (
   "Time" timestamp with time zone NOT NULL,
   "ContextIdentifier" TEXT NOT NULL,
   "CpuTime" DOUBLE PRECISION NULL
 );
SELECT create_hypertable( '"ApiUsages"', 'Time');
CREATE INDEX ix_context_identifier_time ON "ApiUsages" ("ContextIdentifier", "Time" DESC);

-- create minutely summary materialized view
CREATE MATERIALIZED VIEW "ApiUsageSummaryMinutely"("Time", "ContextIdentifier", "CpuTime")
WITH (timescaledb.continuous)
AS SELECT
    time_bucket(INTERVAL '1 minute', "Time"),
    "ContextIdentifier",
    AVG("CpuTime")
FROM "ApiUsages"
GROUP BY "ContextIdentifier", time_bucket(INTERVAL '1 minute', "Time");

CREATE INDEX ix_context_identifier_time_minutely ON "ApiUsageSummaryMinutely" ("ContextIdentifier", "Time" DESC);
-- add raw data retention policy
SELECT add_retention_policy('"ApiUsages"', INTERVAL '5 minutes');

-- add summary data continous aggregate policy
SELECT add_continuous_aggregate_policy('"ApiUsageSummaryMinutely"',
    start_offset => INTERVAL '3 minutes',
    end_offset => INTERVAL '1 minute',
    schedule_interval => INTERVAL '1 minute');
