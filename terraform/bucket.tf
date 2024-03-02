locals {
  bucket_name = "movie-recognizer"
}

resource "yandex_storage_bucket" "bucket" {
  bucket = local.bucket_name
  access_key = var.deployer_access_key
  secret_key = var.deployer_secret_key

  grant {
    id          = yandex_iam_service_account.sa_bucket.id
    type        = "CanonicalUser"
    permissions = ["READ", "WRITE"]
  }

  anonymous_access_flags {
    read = true
    list = false
  }
}

resource "yandex_iam_service_account" "sa_bucket" {
  name        = "${local.bucket_name}-sa"
  description = "Service account to manage ${local.bucket_name}"
}

resource "yandex_iam_service_account_static_access_key" "sa_key_bucket" {
  service_account_id = yandex_iam_service_account.sa_bucket.id
}