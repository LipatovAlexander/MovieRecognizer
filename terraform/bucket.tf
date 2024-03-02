locals {
  bucket_name = "movie-recognizer"
}

resource "yandex_storage_bucket" "bucket" {
  bucket = locals.bucket_name
  access_key = var.deployer_access_key
  secret_key = var.deployer_secret_key

  grant {
    id          = yandex_iam_service_account.sa_bucket.id
    type        = "CanonicalUser"
    permissions = ["READ", "WRITE"]
  }
}

resource "yandex_iam_service_account" "sa_bucket" {
  name        = "${locals.bucket_name}-sa"
  description = "Service account to manage ${locals.bucket_name}"
}

resource "yandex_iam_service_account_static_access_key" "sa_key_bucket" {
  service_account_id = yandex_iam_service_account.sa_bucket.id
}