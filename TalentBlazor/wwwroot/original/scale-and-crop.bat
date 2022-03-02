for %%f in (*.jpg) do call ffmpeg -i "%%f"  -vf "scale=300:300:force_original_aspect_ratio=increase,crop=300:300" "../profiles/%%f"
