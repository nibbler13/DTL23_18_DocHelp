-- `default`.DTL23_GENERAL_STANDARDS definition

CREATE TABLE default.DTL23_GENERAL_STANDARDS
(

    `ID` String,

    `ORDER_NAME` String,

    `GROUP` Nullable(String),

    `MKB` String,

    `SECTION` Nullable(String),

    `TYPE` Nullable(String),

    `SERVICE_CODE` String,

    `SERVICE_NAME` String,

    `APPLICATION_FREQUENCY_INDEX` Float32,

    `RATE_OF_SUBMITTING` Float32,

    `COMMENT` String,

    `CREATE_DATETIME` DateTime
)
ENGINE = ReplacingMergeTree(CREATE_DATETIME)
ORDER BY ID
SETTINGS index_granularity = 8192;