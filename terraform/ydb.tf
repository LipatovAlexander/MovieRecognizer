resource "yandex_ydb_database_serverless" "main-db" {
  name = "main-db"

  serverless_database {
    enable_throttling_rcu_limit = false
    provisioned_rcu_limit       = 10
    storage_size_limit          = 50
    throttling_rcu_limit        = 0
  }
}

resource "yandex_ydb_database_iam_binding" "api-editor" {
  database_id = yandex_ydb_database_serverless.main-db.id
  role        = "ydb.editor"

  members = [
    "serviceAccount:${yandex_iam_service_account.api-sa.id}",
    "serviceAccount:${yandex_iam_service_account.receive-video-handler-sa.id}",
  ]
}

resource "yandex_ydb_table" "movie-recognition" {
  path              = "movie-recognition"
  connection_string = yandex_ydb_database_serverless.main-db.ydb_full_endpoint

  column {
    name     = "id"
    type     = "Utf8"
    not_null = true
  }
  column {
    name     = "video_url"
    type     = "Utf8"
    not_null = true
  }
  column {
    name     = "created_at"
    type     = "Datetime"
    not_null = true
  }
  column {
    name     = "status"
    type     = "Utf8"
    not_null = true
  }
  column {
    name     = "video_id"
    type     = "Utf8"
    not_null = false
  }

  primary_key = ["id"]
}

resource "yandex_ydb_table" "video" {
  path              = "video"
  connection_string = yandex_ydb_database_serverless.main-db.ydb_full_endpoint

  column {
    name     = "id"
    type     = "Utf8"
    not_null = true
  }
  column {
    name     = "external_id"
    type     = "Utf8"
    not_null = true
  }
  column {
    name     = "title"
    type     = "Utf8"
    not_null = true
  }
  column {
    name     = "author"
    type     = "Utf8"
    not_null = true
  }
  column {
    name     = "duration"
    type     = "Interval"
    not_null = true
  }

  primary_key = ["id"]
}
