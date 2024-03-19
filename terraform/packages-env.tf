locals {
  data_env = {
    YDB_ENDPOINT      = "grpcs://${yandex_ydb_database_serverless.main-db.ydb_api_endpoint}"
    YDB_DATABASE_PATH = yandex_ydb_database_serverless.main-db.database_path
  }

  message_queue_env = {
    AWS_SQS_SERVICE_URL = "https://message-queue.api.cloud.yandex.net"
    RECEIVE_VIDEO_QUEUE = module.receive-video-handler.queue_id
    PROCESS_VIDEO_QUEUE = module.process-video-handler.queue_id
  }

  file_storage_env = {
    AWS_S3_SERVICE_URL = "https://s3.yandexcloud.net"
    S3_BUCKET          = yandex_storage_bucket.bucket.bucket
  }
}