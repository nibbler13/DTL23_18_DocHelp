-- `default`.DTL23_SERVICE_COMPARISON definition

CREATE TABLE default.DTL23_SERVICE_COMPARISON
(

    `ID` UInt16,

    `NAME` String,

    `BATCH_APPOINTMENT_ID` Array(UInt16),

    `GENERAL_STANDARDS_ID` Array(UInt16),

    `CREATE_DATETIME` DateTime
)
ENGINE = ReplacingMergeTree(CREATE_DATETIME)
ORDER BY ID
SETTINGS index_granularity = 8192;