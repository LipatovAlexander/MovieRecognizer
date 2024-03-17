locals {
  data_env = {
    YDB_ENDPOINT      = "grpcs://${yandex_ydb_database_serverless.main-db.ydb_api_endpoint}"
    YDB_DATABASE_PATH = yandex_ydb_database_serverless.main-db.database_path
  }

  message_queue_env = {
    AWS_SQS_SERVICE_URL = "https://message-queue.api.cloud.yandex.net"
    RECEIVE_VIDEO_QUEUE = yandex_message_queue.receive-video-handler-queue.id
    PROCESS_VIDEO_QUEUE = yandex_message_queue.process-video-handler-queue.id
  }
}