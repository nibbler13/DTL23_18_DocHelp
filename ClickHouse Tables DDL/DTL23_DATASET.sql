-- `default`.DTL23_DATASET definition

CREATE TABLE default.DTL23_DATASET
(

    `ID` String,

    `SEX` String,

    `BIRTHDAY` Date32,

    `CLIENT_ID` String,

    `MKB` String,

    `DIAGNOSIS` String,

    `TREAT_DATE` Date,

    `DOCTOR_POSITION` String,

    `REFERRALS` Nullable(String),

    `CREATE_DATETIME` DateTime
)
ENGINE = ReplacingMergeTree(CREATE_DATETIME)
ORDER BY ID
SETTINGS index_granularity = 8192;