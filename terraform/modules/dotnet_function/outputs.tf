output "service_account_id" {
  value = yandex_iam_service_account.sa.id
}

output "queue_id" {
  value = yandex_message_queue.queue.id
}