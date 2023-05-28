-- `default`.DTL23_BATCH_APPOINTMENTS definition

CREATE TABLE default.DTL23_BATCH_APPOINTMENTS
(

    `ID` String,

    `SECTION` String,

    `GROUP` String,

    `MKB` String,

    `ORDER` Int16,

    `TYPE` String,

    `SERVICE` String,

    `NECESSITY` String,

    `COMMENT` Nullable(String),

    `CREATE_DATETIME` DateTime
)
ENGINE = ReplacingMergeTree(CREATE_DATETIME)
ORDER BY ID
SETTINGS index_granularity = 8192;