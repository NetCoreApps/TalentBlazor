#!/usr/bin/env bash
while IFS=" " read -r filename url
do
  curl -v "$url" -o "$filename"
done < unsplash-photos.txt