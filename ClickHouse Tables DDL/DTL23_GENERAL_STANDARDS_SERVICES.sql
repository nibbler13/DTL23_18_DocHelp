-- `default`.DTL23_GENERAL_STANDARDS_SERVICES definition

CREATE TABLE default.DTL23_GENERAL_STANDARDS_SERVICES
(

    `ID` UInt16,

    `NAME` String,

    `CREATE_DATETIME` DateTime
)
ENGINE = ReplacingMergeTree(CREATE_DATETIME)
ORDER BY ID
SETTINGS index_granularity = 8192;